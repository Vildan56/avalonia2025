using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using Project2025.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reactive.Linq;
using Avalonia.Threading;
using System;

namespace Project2025.ViewModels
{
    public class RealtorViewModel : ReactiveObject
    {
        public ObservableCollection<Realtor> Realtors { get; } = new();
        public ObservableCollection<Realtor> FilteredRealtors { get; } = new();

        private Realtor? _selectedRealtor;
        public Realtor? SelectedRealtor
        {
            get => _selectedRealtor;
            set => this.RaiseAndSetIfChanged(ref _selectedRealtor, value);
        }

        public bool HasSelectedRealtor => SelectedRealtor != null;
        public bool CanDeleteRealtor => HasSelectedRealtor && !SelectedRealtor.HasNeedsOrOffers;

        private string? _fioSearch;
        public string? FioSearch
        {
            get => _fioSearch;
            set => this.RaiseAndSetIfChanged(ref _fioSearch, value);
        }

        public ReactiveCommand<Unit, Unit> AddRealtorCommand { get; }
        public ReactiveCommand<Unit, Unit> EditRealtorCommand { get; }
        public ReactiveCommand<Unit, Task> DeleteRealtorCommand { get; }
        public ReactiveCommand<Unit, Unit> SearchRealtorsCommand { get; }

        public RealtorViewModel()
        {
            AddRealtorCommand = ReactiveCommand.Create(AddRealtor);
            EditRealtorCommand = ReactiveCommand.Create(EditRealtor,
                this.WhenAnyValue(vm => vm.HasSelectedRealtor));
            DeleteRealtorCommand = ReactiveCommand.Create(DeleteRealtor,
                this.WhenAnyValue(vm => vm.CanDeleteRealtor));
            SearchRealtorsCommand = ReactiveCommand.Create(SearchRealtors);

            this.WhenAnyValue(vm => vm.SelectedRealtor)
                .Subscribe(_ => 
                {
                    this.RaisePropertyChanged(nameof(HasSelectedRealtor));
                    this.RaisePropertyChanged(nameof(CanDeleteRealtor));
                });

            // Загрузка данных
            _ = LoadRealtorsAsync();
        }

        private async Task LoadRealtorsAsync()
        {
            try
            {
                using (var db = new Project2025.AppDbContext())
                {
                    var realtors = await db.Realtors.AsNoTracking().ToListAsync();
                    
                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        Realtors.Clear();
                        foreach (var realtor in realtors)
                        {
                            Realtors.Add(realtor);
                        }
                        UpdateFilteredRealtors();
                    });
                }
            }
            catch (Exception ex)
            {
                // TODO: Add proper error handling/logging
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    // You might want to show an error message to the user here
                });
            }
        }

        private void AddRealtor() => _ = ShowEditorAsync(new Realtor());
        private void EditRealtor() => _ = ShowEditorAsync(SelectedRealtor!);
        
        private async Task DeleteRealtor()
        {
            if (SelectedRealtor != null)
            {
                try
                {
                    using (var db = new Project2025.AppDbContext())
                    {
                        var dbObj = await db.Realtors.FindAsync(SelectedRealtor.Id);
                        if (dbObj != null)
                        {
                            db.Realtors.Remove(dbObj);
                            await db.SaveChangesAsync();
                        }
                    }

                    Realtors.Remove(SelectedRealtor);
                    UpdateFilteredRealtors();
                }
                catch (Exception ex)
                {
                    // TODO: Add proper error handling/logging
                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        // You might want to show an error message to the user here
                    });
                }
            }
        }

        private async Task ShowEditorAsync(Realtor realtor)
        {
            try
            {
                var editor = new Project2025.Views.RealtorEditor(realtor);

                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    var result = await editor.ShowDialog<string>(desktop.MainWindow);
                    if (result == "save")
                    {
                        using (var db = new Project2025.AppDbContext())
                        {
                            if (realtor.Id == 0) // New realtor
                            {
                                db.Realtors.Add(realtor);
                                await db.SaveChangesAsync();
                                Realtors.Add(realtor);
                            }
                            else // Existing realtor
                            {
                                var dbObj = await db.Realtors.FindAsync(realtor.Id);
                                if (dbObj != null)
                                {
                                    db.Entry(dbObj).CurrentValues.SetValues(realtor);
                                    await db.SaveChangesAsync();
                                    var index = Realtors.IndexOf(Realtors.First(r => r.Id == realtor.Id));
                                    if (index >= 0)
                                    {
                                        Realtors[index] = realtor;
                                    }
                                }
                            }
                        }
                        UpdateFilteredRealtors();
                        SelectedRealtor = realtor;
                    }
                    else if (result == "delete")
                    {
                        if (realtor.Id != 0)
                        {
                            await DeleteRealtor();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO: Add proper error handling/logging
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    // You might want to show an error message to the user here
                });
            }
        }

        private void SearchRealtors() => UpdateFilteredRealtors();
        
        private void UpdateFilteredRealtors()
        {
            var currentSelectedId = SelectedRealtor?.Id;
            
            FilteredRealtors.Clear();
            var filtered = Realtors.AsEnumerable();
            
            if (!string.IsNullOrWhiteSpace(FioSearch))
            {
                filtered = filtered.Where(r => 
                    LevenshteinDistance((r.FullName ?? "").ToLower(), FioSearch.ToLower()) <= 3);
            }
            
            foreach (var item in filtered)
                FilteredRealtors.Add(item);
            
            // Восстанавливаем выделение
            if (currentSelectedId.HasValue)
            {
                SelectedRealtor = FilteredRealtors.FirstOrDefault(r => r.Id == currentSelectedId.Value);
            }
        }
        
        private int LevenshteinDistance(string s, string t)
        {
            if (string.IsNullOrEmpty(s)) return string.IsNullOrEmpty(t) ? 0 : t.Length;
            if (string.IsNullOrEmpty(t)) return s.Length;
            var d = new int[s.Length + 1, t.Length + 1];
            for (int i = 0; i <= s.Length; i++) d[i, 0] = i;
            for (int j = 0; j <= t.Length; j++) d[0, j] = j;
            for (int i = 1; i <= s.Length; i++)
                for (int j = 1; j <= t.Length; j++)
                {
                    int cost = s[i - 1] == t[j - 1] ? 0 : 1;
                    d[i, j] = new[]
                    {
                        d[i - 1, j] + 1,
                        d[i, j - 1] + 1,
                        d[i - 1, j - 1] + cost
                    }.Min();
                }
            return d[s.Length, t.Length];
        }
    }
}