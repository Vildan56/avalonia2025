using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Project2025.Models;

namespace Project2025.Views
{
    public class RealEstateEditor : Window
    {
        private readonly RealEstate _property;

        public RealEstateEditor(RealEstate property)
        {
            _property = property;
            InitializeComponent();
            DataContext = this;
        }

        private void InitializeComponent()
        {
            Width = 400;
            Height = 350;
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
            var typeBox = new TextBox { Watermark = "Property Type" };
            typeBox.Bind(TextBox.TextProperty, new Binding("Type") { Source = _property, Mode = BindingMode.TwoWay });
            panel.Children.Add(typeBox);
            panel.Children.Add(new TextBlock
            {
                Text = "Address",
                FontWeight = Avalonia.Media.FontWeight.Bold,
                Margin = new Thickness(0, 10, 0, 5)
            });
            var cityBox = new TextBox { Watermark = "City" };
            cityBox.Bind(TextBox.TextProperty, new Binding("Address.City") { Source = _property, Mode = BindingMode.TwoWay });
            panel.Children.Add(cityBox);
            var streetBox = new TextBox { Watermark = "Street" };
            streetBox.Bind(TextBox.TextProperty, new Binding("Address.Street") { Source = _property, Mode = BindingMode.TwoWay });
            panel.Children.Add(streetBox);
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
            var latBox = new TextBox { Watermark = "Latitude (-90..90)" };
            latBox.Bind(TextBox.TextProperty, new Binding("Coordinates.Latitude")
            {
                Source = _property,
                Mode = BindingMode.TwoWay,
                StringFormat = "{0:0.00000}"
            });
            coordPanel.Children.Add(latBox);
            var lonBox = new TextBox { Watermark = "Longitude (-180..180)" };
            lonBox.Bind(TextBox.TextProperty, new Binding("Coordinates.Longitude")
            {
                Source = _property,
                Mode = BindingMode.TwoWay,
                StringFormat = "{0:0.00000}"
            });
            coordPanel.Children.Add(lonBox);
            panel.Children.Add(coordPanel);
            var saveButton = new Button
            {
                Content = "Save",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
                Width = 80
            };
            saveButton.Click += (s, e) => Close(true);
            panel.Children.Add(saveButton);
            Content = panel;
        }
    }
} 