using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Project2025.Models;
using ReactiveUI;
using System;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;


namespace Project2025.Views
{
    public class RealEstateEditor : Window
    {
        private readonly RealEstate _property;
        private TextBox typeBox, cityBox, streetBox, latBox, lonBox;
        private Button saveButton, deleteButton;

        public RealEstateEditor(RealEstate property)
        {
            _property = property;
            InitializeComponent();
            DataContext = this;
        }

        private void InitializeComponent()
        {
            Width = 400;
            Height = 370;
            Title = "Real Estate Editor";
            var panel = new StackPanel
            {
                Margin = new Thickness(15),
                Spacing = 10
            };
            panel.Children.Add(new TextBlock
            {
                Text = "Property Details",
                FontSize = 16,
                FontWeight = Avalonia.Media.FontWeight.Bold
            });
            typeBox = new TextBox { Watermark = "Property Type" };
            typeBox.Bind(TextBox.TextProperty, new Binding("Type") { Source = _property, Mode = BindingMode.TwoWay });
            panel.Children.Add(typeBox);
            var typeError = new TextBlock
            {
                Text = "Укажите тип недвижимости",
                Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red),
                IsVisible = string.IsNullOrWhiteSpace(_property.Type)
            };
            panel.Children.Add(typeError);

            panel.Children.Add(new TextBlock
            {
                Text = "Address",
                FontWeight = Avalonia.Media.FontWeight.Bold,
                Margin = new Thickness(0, 10, 0, 5)
            });
            cityBox = new TextBox { Watermark = "City" };
            cityBox.Bind(TextBox.TextProperty, new Binding("Address.City") { Source = _property, Mode = BindingMode.TwoWay });
            panel.Children.Add(cityBox);
            var cityError = new TextBlock
            {
                Text = "Укажите город",
                Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red),
                IsVisible = string.IsNullOrWhiteSpace(_property.Address?.City)
            };
            panel.Children.Add(cityError);
            streetBox = new TextBox { Watermark = "Street" };
            streetBox.Bind(TextBox.TextProperty, new Binding("Address.Street") { Source = _property, Mode = BindingMode.TwoWay });
            panel.Children.Add(streetBox);
            var streetError = new TextBlock
            {
                Text = "Укажите улицу",
                Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red),
                IsVisible = string.IsNullOrWhiteSpace(_property.Address?.Street)
            };
            panel.Children.Add(streetError);
            var addressPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 5 };
            var houseBox = new TextBox { Watermark = "Building", Width = 150 };
            houseBox.Bind(TextBox.TextProperty, new Binding("Address.HouseNumber") { Source = _property, Mode = BindingMode.TwoWay });
            addressPanel.Children.Add(houseBox);
            var apartmentBox = new TextBox { Watermark = "Apartment", Width = 150 };
            apartmentBox.Bind(TextBox.TextProperty, new Binding("Address.ApartmentNumber") { Source = _property, Mode = BindingMode.TwoWay });
            addressPanel.Children.Add(apartmentBox);
            panel.Children.Add(addressPanel);
            panel.Children.Add(new TextBlock
            {
                Text = "Coordinates",
                FontWeight = Avalonia.Media.FontWeight.Bold,
                Margin = new Thickness(0, 10, 0, 5)
            });
            var coordPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 5 };
            latBox = new TextBox { Watermark = "Latitude (-90..90)" };
            latBox.Bind(TextBox.TextProperty, new Binding("Coordinates.Latitude")
            {
                Source = _property,
                Mode = BindingMode.TwoWay,
                StringFormat = "{0:0.00000}"
            });
            coordPanel.Children.Add(latBox);
            lonBox = new TextBox { Watermark = "Longitude (-180..180)" };
            lonBox.Bind(TextBox.TextProperty, new Binding("Coordinates.Longitude")
            {
                Source = _property,
                Mode = BindingMode.TwoWay,
                StringFormat = "{0:0.00000}"
            });
            coordPanel.Children.Add(lonBox);
            panel.Children.Add(coordPanel);
            var latError = new TextBlock
            {
                Text = "Широта должна быть от -90 до 90",
                Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red),
                IsVisible = _property.Coordinates == null || _property.Coordinates.Latitude < -90 || _property.Coordinates.Latitude > 90
            };
            panel.Children.Add(latError);
            var lonError = new TextBlock
            {
                Text = "Долгота должна быть от -180 до 180",
                Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red),
                IsVisible = _property.Coordinates == null || _property.Coordinates.Longitude < -180 || _property.Coordinates.Longitude > 180
            };
            panel.Children.Add(lonError);
            var warning = new TextBlock
            {
                Text = "* Заполните обязательные поля: тип, город, улица, координаты (-90..90, -180..180)",
                Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red),
                IsVisible = !IsValid()
            };
            panel.Children.Add(warning);
            var buttonPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 10, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right };
            saveButton = new Button
            {
                Content = "Save",
                Width = 80,
                IsEnabled = IsValid()
            };
            saveButton.Click += (s, e) =>
            {
                if (IsValid())
                {
                    Close("save");
                }
                else
                {
                    FocusFirstInvalid();
                }
            };
            buttonPanel.Children.Add(saveButton);
            deleteButton = new Button
            {
                Content = "Delete",
                Width = 80,
                IsVisible = _property.Id != 0
            };
            deleteButton.Click += async (s, e) =>
            {
                var box = MessageBoxManager.GetMessageBoxStandard(
                    "Подтверждение удаления",
                    "Вы уверены, что хотите удалить?",
                    ButtonEnum.YesNo

                );
                var result = await box.ShowWindowDialogAsync(this);
                if (result == ButtonResult.Yes)
                {
                    Close("delete");
                }
            };
            buttonPanel.Children.Add(deleteButton);
            panel.Children.Add(buttonPanel);
            Content = panel;

            void UpdateValidation(object? sender, EventArgs? e)
            {
                saveButton.IsEnabled = IsValid();
                warning.IsVisible = !IsValid();
                typeError.IsVisible = string.IsNullOrWhiteSpace(_property.Type);
                cityError.IsVisible = string.IsNullOrWhiteSpace(_property.Address?.City);
                streetError.IsVisible = string.IsNullOrWhiteSpace(_property.Address?.Street);
                latError.IsVisible = _property.Coordinates == null || _property.Coordinates.Latitude < -90 || _property.Coordinates.Latitude > 90;
                lonError.IsVisible = _property.Coordinates == null || _property.Coordinates.Longitude < -180 || _property.Coordinates.Longitude > 180;
            }
            typeBox.PropertyChanged += UpdateValidation;
            cityBox.PropertyChanged += UpdateValidation;
            streetBox.PropertyChanged += UpdateValidation;
            latBox.PropertyChanged += UpdateValidation;
            lonBox.PropertyChanged += UpdateValidation;
            // Автофокус при открытии
            FocusFirstInvalid();
        }
        private void FocusFirstInvalid()
        {
            if (string.IsNullOrWhiteSpace(_property.Type))
                typeBox.Focus();
            else if (_property.Address == null || string.IsNullOrWhiteSpace(_property.Address.City))
                cityBox.Focus();
            else if (string.IsNullOrWhiteSpace(_property.Address.Street))
                streetBox.Focus();
            else if (_property.Coordinates == null || _property.Coordinates.Latitude < -90 || _property.Coordinates.Latitude > 90)
                latBox.Focus();
            else if (_property.Coordinates == null || _property.Coordinates.Longitude < -180 || _property.Coordinates.Longitude > 180)
                lonBox.Focus();
        }
        private bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(_property.Type)
                && _property.Address != null
                && !string.IsNullOrWhiteSpace(_property.Address.City)
                && !string.IsNullOrWhiteSpace(_property.Address.Street)
                && _property.Coordinates != null
                && _property.Coordinates.Latitude >= -90 && _property.Coordinates.Latitude <= 90
                && _property.Coordinates.Longitude >= -180 && _property.Coordinates.Longitude <= 180;
        }
    }
} 