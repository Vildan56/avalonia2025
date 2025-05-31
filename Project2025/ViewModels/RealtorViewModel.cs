using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using Project2025.Models;

namespace Project2025.ViewModels
{
    public class RealtorViewModel : ReactiveObject
    {
        public ObservableCollection<Realtor> Realtors { get; } = new();

        private Realtor? _selectedRealtor;
        public Realtor? SelectedRealtor
        {
            get => _selectedRealtor;
            set => this.RaiseAndSetIfChanged(ref _selectedRealtor, value);
        }

        public bool HasSelectedRealtor => SelectedRealtor != null;
        public bool CanDeleteRealtor => HasSelectedRealtor && !SelectedRealtor.HasNeedsOrOffers;

        public ReactiveCommand<Unit, Unit> AddRealtorCommand { get; }
        public ReactiveCommand<Unit, Unit> EditRealtorCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteRealtorCommand { get; }

        public RealtorViewModel()
        {
            AddRealtorCommand = ReactiveCommand.Create(AddRealtor);
            EditRealtorCommand = ReactiveCommand.Create(EditRealtor,
                this.WhenAnyValue(vm => vm.HasSelectedRealtor));
            DeleteRealtorCommand = ReactiveCommand.Create(DeleteRealtor,
                this.WhenAnyValue(vm => vm.CanDeleteRealtor));

            Realtors.Add(new Realtor
            {
                Id = 1,
                LastName = "Petrova",
                FirstName = "Anna",
                MiddleName = "Sergeevna",
                CommissionShare = 45
            });
        }

        private void AddRealtor() => ShowEditor(new Realtor());
        private void EditRealtor() => ShowEditor(SelectedRealtor!);

        private async void ShowEditor(Realtor realtor)
        {
            var editor = new Project2025.Views.RealtorEditor(realtor);

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (await editor.ShowDialog<bool>(desktop.MainWindow))
                {
                    if (realtor.Id == 0)
                    {
                        realtor.Id = Realtors.Count + 1;
                        Realtors.Add(realtor);
                    }
                }
            }
        }

        private void DeleteRealtor()
        {
            if (SelectedRealtor != null) Realtors.Remove(SelectedRealtor);
        }
    }
} 