using ReactiveUI;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Reactive;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using System.Linq;
using Project2025.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Reactive.Linq;
using Avalonia.Threading;
using System;
using System.Diagnostics;

namespace Project2025.ViewModels
{
    public class RealEstateViewModel : ReactiveObject
    {
        public ObservableCollection<RealEstate> Properties { get; } = new();
        public ObservableCollection<RealEstate> FilteredProperties { get; } = new();
        public List<string> PropertyTypes { get; } = new() { "Apartment", "House", "Land", "Commercial" };

        private RealEstate? _selectedProperty;

        public RealEstate? SelectedProperty
        {
            get => _selectedProperty;
            set => this.RaiseAndSetIfChanged(ref _selectedProperty, value);
        }

        public bool HasSelectedProperty => SelectedProperty != null;
        public bool CanDeleteProperty => HasSelectedProperty && !SelectedProperty.HasOffers;

        private string? _selectedTypeFilter;

        public string? SelectedTypeFilter
        {
            get => _selectedTypeFilter;
            set => this.RaiseAndSetIfChanged(ref _selectedTypeFilter, value);
        }

        private string? _addressFilter;

        public string? AddressFilter
        {
            get => _addressFilter;
            set => this.RaiseAndSetIfChanged(ref _addressFilter, value);
        }

        public ReactiveCommand<Unit, Unit> AddPropertyCommand { get; }
        public ReactiveCommand<Unit, Unit> EditPropertyCommand { get; }
        public ReactiveCommand<Unit, Unit> DeletePropertyCommand { get; }
        public ReactiveCommand<Unit, Unit> ApplyFiltersCommand { get; }

        public RealEstateViewModel()
        {
            AddPropertyCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                await ShowEditorAsync(new RealEstate());
                return Unit.Default;
            });

            EditPropertyCommand = ReactiveCommand.CreateFromTask(
                async () =>
                {
                    await ShowEditorAsync(SelectedProperty!);
                    return Unit.Default;
                },
                this.WhenAnyValue(vm => vm.HasSelectedProperty));

            DeletePropertyCommand = ReactiveCommand.CreateFromTask(
                async () =>
                {
                    await DeleteProperty();
                    return Unit.Default;
                },
                this.WhenAnyValue(vm => vm.CanDeleteProperty));

            ApplyFiltersCommand = ReactiveCommand.Create(ApplyFilters);

            this.WhenAnyValue(vm => vm.SelectedProperty)
                .Subscribe(_ =>
                {
                    this.RaisePropertyChanged(nameof(HasSelectedProperty));
                    this.RaisePropertyChanged(nameof(CanDeleteProperty));
                });

            // Загрузка данных
            _ = LoadPropertiesAsync();
        }

        private async Task LoadPropertiesAsync()
        {
            try
            {
                using (var db = new Project2025.AppDbContext())
                {
                    var properties = await db.PropertyObjects
                        .AsNoTracking()
                        .Include(p => p.PropertyType)
                        .Select(p => new RealEstate
                        {
                            Id = p.Id,
                            Type = p.PropertyType != null ? p.PropertyType.Name : string.Empty,
                            Address = new Address
                            {
                                City = p.City,
                                Street = p.Street,
                                HouseNumber = p.House,
                                ApartmentNumber = p.Apartment
                            },
                            Coordinates = new Coordinates
                            {
                                Latitude = p.Latitude,
                                Longitude = p.Longitude
                            }
                        })
                        .ToListAsync();

                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        Properties.Clear();
                        foreach (var property in properties)
                        {
                            Properties.Add(property);
                        }

                        UpdateFilteredProperties();
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка при загрузке объектов недвижимости: {ex}");
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    // TODO: Добавить уведомление пользователя об ошибке
                });
            }
        }

        private async Task DeleteProperty()
        {
            if (SelectedProperty != null)
            {
                try
                {
                    using (var db = new Project2025.AppDbContext())
                    {
                        var dbObj = await db.PropertyObjects.FindAsync(SelectedProperty.Id);
                        if (dbObj != null)
                        {
                            db.PropertyObjects.Remove(dbObj);
                            await db.SaveChangesAsync();
                        }
                    }

                    Properties.Remove(SelectedProperty);
                    UpdateFilteredProperties();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Ошибка при удалении объекта недвижимости: {ex}");
                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        // TODO: Добавить уведомление пользователя об ошибке
                    });
                }
            }
        }

        private async Task ShowEditorAsync(RealEstate property)
        {
            var editor = new Project2025.Views.RealEstateEditor(property);

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var result = await editor.ShowDialog<string>(desktop.MainWindow);
                if (result == "save")
                {
                    if (property.Id == 0)
                    {
                        property.Id = Properties.Count + 1;
                        Properties.Add(property);
                    }
                    UpdateFilteredProperties();
                    SelectedProperty = property;
                }
                else if (result == "delete")
                {
                    if (property.Id != 0)
                    {
                        await DeleteProperty();
                    }
                }
            }
        }

        private void ApplyFilters() => UpdateFilteredProperties();

        private void UpdateFilteredProperties()
        {
            var currentSelectedId = SelectedProperty?.Id;

            FilteredProperties.Clear();
            var filtered = Properties.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SelectedTypeFilter))
                filtered = filtered.Where(p => p.Type == SelectedTypeFilter);

            if (!string.IsNullOrWhiteSpace(AddressFilter))
                filtered = filtered.Where(p =>
                    (p.Address?.ToString() ?? "").ToLower().Contains(AddressFilter.ToLower()));

            foreach (var item in filtered)
                FilteredProperties.Add(item);

            // Восстанавливаем выделение
            if (currentSelectedId.HasValue)
            {
                SelectedProperty = FilteredProperties.FirstOrDefault(p => p.Id == currentSelectedId.Value);
            }
        }
    }
}