using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Project2025.Models;

namespace Project2025.Views
{
    public class ClientEditor : Window
    {
        private readonly Client _client;
        public ClientEditor(Client client)
        {
            _client = client;
            InitializeComponent();
            DataContext = this;
        }
        private void InitializeComponent()
        {
            Width = 300;
            Height = 300;
            Title = "Client Editor";
            var panel = new StackPanel
            {
                Margin = new Thickness(15),
                Spacing = 10
            };
            panel.Children.Add(new TextBlock
            {
                Text = "Client Details",
                FontSize = 16,
                FontWeight = Avalonia.Media.FontWeight.Bold
            });
            var namePanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 5 };
            var firstNameBox = new TextBox { Watermark = "First Name" };
            firstNameBox.Bind(TextBox.TextProperty, new Binding("FirstName") { Source = _client, Mode = BindingMode.TwoWay });
            namePanel.Children.Add(firstNameBox);
            var lastNameBox = new TextBox { Watermark = "Last Name" };
            lastNameBox.Bind(TextBox.TextProperty, new Binding("LastName") { Source = _client, Mode = BindingMode.TwoWay });
            namePanel.Children.Add(lastNameBox);
            panel.Children.Add(namePanel);
            var middleNameBox = new TextBox { Watermark = "Middle Name" };
            middleNameBox.Bind(TextBox.TextProperty, new Binding("MiddleName") { Source = _client, Mode = BindingMode.TwoWay });
            panel.Children.Add(middleNameBox);
            var phoneBox = new TextBox { Watermark = "Phone" };
            phoneBox.Bind(TextBox.TextProperty, new Binding("Phone") { Source = _client, Mode = BindingMode.TwoWay });
            panel.Children.Add(phoneBox);
            var emailBox = new TextBox { Watermark = "Email" };
            emailBox.Bind(TextBox.TextProperty, new Binding("Email") { Source = _client, Mode = BindingMode.TwoWay });
            panel.Children.Add(emailBox);
            var warning = new TextBlock
            {
                Text = "* At least one contact method required",
                Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red),
                IsVisible = !_client.HasContactInfo
            };
            panel.Children.Add(warning);
            var saveButton = new Button
            {
                Content = "Save",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
                Width = 80,
                IsEnabled = _client.HasContactInfo
            };
            saveButton.Click += (s, e) => Close(true);
            panel.Children.Add(saveButton);
            Content = panel;
        }
    }
} 