using ReactiveUI;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Reactive;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using System.Linq;
using Project2025.Models;

namespace Project2025.ViewModels
{
    public class RealEstateViewModel : ReactiveObject
    {
        public ObservableCollection<RealEstate> Properties { get; } = new();
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
            AddPropertyCommand = ReactiveCommand.Create(AddProperty);
            EditPropertyCommand = ReactiveCommand.Create(EditProperty,
                this.WhenAnyValue(vm => vm.HasSelectedProperty));
            DeletePropertyCommand = ReactiveCommand.Create(DeleteProperty,
                this.WhenAnyValue(vm => vm.CanDeleteProperty));
            ApplyFiltersCommand = ReactiveCommand.Create(ApplyFilters);

            Properties.Add(new RealEstate
            {
                Id = 1,
                Type = "Apartment",
                Address = new Address
                {
                    City = "Moscow",
                    Street = "Tverskaya",
                    HouseNumber = "10",
                    ApartmentNumber = "25"
                },
                Coordinates = new Coordinates { Latitude = 55.7558, Longitude = 37.6173 }
            });
        }

        private void AddProperty() => ShowEditor(new RealEstate());
        private void EditProperty() => ShowEditor(SelectedProperty!);

        private async void ShowEditor(RealEstate property)
        {
            var editor = new Project2025.Views.RealEstateEditor(property);

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (await editor.ShowDialog<bool>(desktop.MainWindow))
                {
                    if (property.Id == 0)
                    {
                        property.Id = Properties.Count + 1;
                        Properties.Add(property);
                    }
                }
            }
        }

        private void DeleteProperty()
        {
            if (SelectedProperty != null) Properties.Remove(SelectedProperty);
        }

        private void ApplyFilters()
        {
            // Реализация фильтрации
        }
    }
} 