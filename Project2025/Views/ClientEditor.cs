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
    public class ClientEditor : Window
    {
        private readonly Client _client;
        private TextBox firstNameBox, lastNameBox, middleNameBox, phoneBox, emailBox;
        private Button saveButton, deleteButton;
        public ClientEditor(Client client)
        {
            _client = client;
            InitializeComponent();
            DataContext = this;
        }
        private void InitializeComponent()
        {
            Width = 300;
            Height = 330;
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
            firstNameBox = new TextBox { Watermark = "First Name" };
            firstNameBox.Bind(TextBox.TextProperty, new Binding("FirstName") { Source = _client, Mode = BindingMode.TwoWay });
            namePanel.Children.Add(firstNameBox);
            lastNameBox = new TextBox { Watermark = "Last Name" };
            lastNameBox.Bind(TextBox.TextProperty, new Binding("LastName") { Source = _client, Mode = BindingMode.TwoWay });
            namePanel.Children.Add(lastNameBox);
            panel.Children.Add(namePanel);
            middleNameBox = new TextBox { Watermark = "Middle Name" };
            middleNameBox.Bind(TextBox.TextProperty, new Binding("MiddleName") { Source = _client, Mode = BindingMode.TwoWay });
            panel.Children.Add(middleNameBox);
            var fioError = new TextBlock
            {
                Text = "Заполните хотя бы одно из полей ФИО",
                Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red),
                IsVisible = string.IsNullOrWhiteSpace(_client.FirstName) && string.IsNullOrWhiteSpace(_client.LastName) && string.IsNullOrWhiteSpace(_client.MiddleName)
            };
            panel.Children.Add(fioError);
            phoneBox = new TextBox { Watermark = "Phone" };
            phoneBox.Bind(TextBox.TextProperty, new Binding("Phone") { Source = _client, Mode = BindingMode.TwoWay });
            panel.Children.Add(phoneBox);
            var phoneError = new TextBlock
            {
                Text = "Укажите телефон",
                Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red),
                IsVisible = string.IsNullOrWhiteSpace(_client.Phone) && string.IsNullOrWhiteSpace(_client.Email)
            };
            panel.Children.Add(phoneError);
            emailBox = new TextBox { Watermark = "Email" };
            emailBox.Bind(TextBox.TextProperty, new Binding("Email") { Source = _client, Mode = BindingMode.TwoWay });
            panel.Children.Add(emailBox);
            var emailError = new TextBlock
            {
                Text = "Укажите email",
                Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red),
                IsVisible = string.IsNullOrWhiteSpace(_client.Phone) && string.IsNullOrWhiteSpace(_client.Email)
            };
            panel.Children.Add(emailError);
            var warning = new TextBlock
            {
                Text = "* Заполните хотя бы одно из полей: телефон или email, а также хотя бы одно из ФИО",
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
                IsVisible = _client.Id != 0
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
                fioError.IsVisible = string.IsNullOrWhiteSpace(_client.FirstName) && string.IsNullOrWhiteSpace(_client.LastName) && string.IsNullOrWhiteSpace(_client.MiddleName);
                phoneError.IsVisible = string.IsNullOrWhiteSpace(_client.Phone) && string.IsNullOrWhiteSpace(_client.Email);
                emailError.IsVisible = string.IsNullOrWhiteSpace(_client.Phone) && string.IsNullOrWhiteSpace(_client.Email);
            }
            firstNameBox.PropertyChanged += UpdateValidation;
            lastNameBox.PropertyChanged += UpdateValidation;
            middleNameBox.PropertyChanged += UpdateValidation;
            phoneBox.PropertyChanged += UpdateValidation;
            emailBox.PropertyChanged += UpdateValidation;
            // Автофокус при открытии
            FocusFirstInvalid();
        }
        private void FocusFirstInvalid()
        {
            if (string.IsNullOrWhiteSpace(_client.FirstName) && string.IsNullOrWhiteSpace(_client.LastName) && string.IsNullOrWhiteSpace(_client.MiddleName))
                firstNameBox.Focus();
            else if (string.IsNullOrWhiteSpace(_client.Phone) && string.IsNullOrWhiteSpace(_client.Email))
                phoneBox.Focus();
        }
        private bool IsValid()
        {
            return (!string.IsNullOrWhiteSpace(_client.Phone) || !string.IsNullOrWhiteSpace(_client.Email)) &&
                   (!string.IsNullOrWhiteSpace(_client.FirstName) || !string.IsNullOrWhiteSpace(_client.LastName) || !string.IsNullOrWhiteSpace(_client.MiddleName));
        }
    }
} 