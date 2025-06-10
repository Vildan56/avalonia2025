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
using ReactiveUI.Validation;
using ReactiveUI.Validation.Extensions;
using Avalonia.Data.Converters;
using System.Collections.Generic;

namespace Project2025.Views
{
    public class ClientEditor : Window, IViewFor<Client>
    {
        public static readonly AvaloniaProperty ViewModelProperty =
            AvaloniaProperty.Register<ClientEditor, Client>(nameof(ViewModel));

        public Client ViewModel
        {
            get => (Client)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (Client)value;
        }

        private TextBox firstNameBox, lastNameBox, middleNameBox, phoneBox, emailBox;
        private Button saveButton, deleteButton;
        private TextBlock errorBlock;
        private TextBlock firstNameError, lastNameError, middleNameError, phoneError, emailError, contactError;
        public ClientEditor(Client client)
        {
            ViewModel = client;
            InitializeComponent();
            DataContext = this;
        }
        private void InitializeComponent()
        {
            Width = 300;
            Height = 400;
            Title = "Client Editor";
            var mainPanel = new StackPanel
            {
                Background = Avalonia.Media.Brushes.White,
                Margin = new Thickness(15),
                Spacing = 10
            };
            mainPanel.Children.Insert(0, new Image
            {
                Source = new Avalonia.Media.Imaging.Bitmap("avares://Project2025/Assets/logo.png"),
                Width = 64,
                Height = 64,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 10)
            });
            mainPanel.Children.Insert(1, new TextBlock
            {
                Text = "Добавление/редактирование клиента",
                FontSize = 20,
                FontWeight = Avalonia.Media.FontWeight.Bold,
                Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("#004AFF")),
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 20)
            });
            var namePanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 5 };
            firstNameBox = new TextBox {
                Watermark = "First Name",
                Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("#FFFFFF")),
                Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("#37474F")),
                FontFamily = new Avalonia.Media.FontFamily("Roboto"),
                Height = 36,
                Padding = new Thickness(10),
                Margin = new Thickness(15),
                BorderBrush = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("#ECEFF1")),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(3)
            };
            firstNameBox.Bind(TextBox.TextProperty, new Binding("FirstName") { Mode = BindingMode.TwoWay });
            namePanel.Children.Add(firstNameBox);
            lastNameBox = new TextBox {
                Watermark = "Last Name",
                Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("#FFFFFF")),
                Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("#37474F")),
                FontFamily = new Avalonia.Media.FontFamily("Roboto"),
                Height = 36,
                Padding = new Thickness(10),
                Margin = new Thickness(15),
                BorderBrush = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("#ECEFF1")),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(3)
            };
            lastNameBox.Bind(TextBox.TextProperty, new Binding("LastName") { Mode = BindingMode.TwoWay });
            namePanel.Children.Add(lastNameBox);
            mainPanel.Children.Add(namePanel);
            // Ошибки для имени и фамилии
            var errorPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 5 };
            firstNameError = new TextBlock { Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red) };
            errorPanel.Children.Add(firstNameError);
            lastNameError = new TextBlock { Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red) };
            errorPanel.Children.Add(lastNameError);
            mainPanel.Children.Add(errorPanel);
            middleNameBox = new TextBox {
                Watermark = "Middle Name",
                Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("#FFFFFF")),
                Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("#37474F")),
                FontFamily = new Avalonia.Media.FontFamily("Roboto"),
                Height = 36,
                Padding = new Thickness(10),
                Margin = new Thickness(15),
                BorderBrush = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("#ECEFF1")),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(3)
            };
            middleNameBox.Bind(TextBox.TextProperty, new Binding("MiddleName") { Mode = BindingMode.TwoWay });
            mainPanel.Children.Add(middleNameBox);
            middleNameError = new TextBlock { Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red) };
            mainPanel.Children.Add(middleNameError);
            phoneBox = new TextBox {
                Watermark = "Phone",
                Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("#FFFFFF")),
                Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("#37474F")),
                FontFamily = new Avalonia.Media.FontFamily("Roboto"),
                Height = 36,
                Padding = new Thickness(10),
                Margin = new Thickness(15),
                BorderBrush = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("#ECEFF1")),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(3)
            };
            phoneBox.Bind(TextBox.TextProperty, new Binding("Phone") { Mode = BindingMode.TwoWay });
            mainPanel.Children.Add(phoneBox);
            phoneError = new TextBlock { Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red) };
            mainPanel.Children.Add(phoneError);
            emailBox = new TextBox {
                Watermark = "Email",
                Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("#FFFFFF")),
                Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("#37474F")),
                FontFamily = new Avalonia.Media.FontFamily("Roboto"),
                Height = 36,
                Padding = new Thickness(10),
                Margin = new Thickness(15),
                BorderBrush = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("#ECEFF1")),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(3)
            };
            emailBox.Bind(TextBox.TextProperty, new Binding("Email") { Mode = BindingMode.TwoWay });
            mainPanel.Children.Add(emailBox);
            emailError = new TextBlock { Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red) };
            mainPanel.Children.Add(emailError);
            contactError = new TextBlock { Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red) };
            mainPanel.Children.Add(contactError);
            var buttonPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 10, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right };
            saveButton = new Button {
                Content = "Save",
                Width = 80,
                Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("#004AFF")),
                Foreground = Avalonia.Media.Brushes.White,
                FontFamily = new Avalonia.Media.FontFamily("Roboto"),
                FontWeight = Avalonia.Media.FontWeight.Bold,
                Height = 36,
                Padding = new Thickness(10),
                Margin = new Thickness(15),
                CornerRadius = new CornerRadius(3),
                BorderThickness = new Thickness(0)
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
            deleteButton = new Button {
                Content = "Delete",
                Width = 80,
                Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("#E3002C")),
                Foreground = Avalonia.Media.Brushes.White,
                FontFamily = new Avalonia.Media.FontFamily("Roboto"),
                FontWeight = Avalonia.Media.FontWeight.Bold,
                Height = 36,
                Padding = new Thickness(10),
                Margin = new Thickness(15),
                CornerRadius = new CornerRadius(3),
                BorderThickness = new Thickness(0)
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
            mainPanel.Children.Add(buttonPanel);
            Content = mainPanel;
            // Ручная валидация
            firstNameBox.PropertyChanged += UpdateValidation;
            lastNameBox.PropertyChanged += UpdateValidation;
            middleNameBox.PropertyChanged += UpdateValidation;
            phoneBox.PropertyChanged += UpdateValidation;
            emailBox.PropertyChanged += UpdateValidation;
            UpdateValidation(null, null);
            // Автофокус при открытии
            FocusFirstInvalid();
        }
        private void UpdateValidation(object? sender, AvaloniaPropertyChangedEventArgs? e)
        {
            firstNameError.Text = string.IsNullOrWhiteSpace(ViewModel.FirstName) ? "Имя обязательно" : "";
            lastNameError.Text = string.IsNullOrWhiteSpace(ViewModel.LastName) ? "Фамилия обязательна" : "";
            middleNameError.Text = string.IsNullOrWhiteSpace(ViewModel.LastName) ? "Отчество обязательна" : ""; // Можно добавить свою логику
            phoneError.Text = !string.IsNullOrWhiteSpace(ViewModel.Phone) && !System.Text.RegularExpressions.Regex.IsMatch(ViewModel.Phone, @"^\\+?\\d{10,15}$") ? "Некорректный телефон" : "";
            emailError.Text = !string.IsNullOrWhiteSpace(ViewModel.Email) && !System.Text.RegularExpressions.Regex.IsMatch(ViewModel.Email, @"^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$") ? "Некорректный email" : "";
            contactError.Text = string.IsNullOrWhiteSpace(ViewModel.Phone) && string.IsNullOrWhiteSpace(ViewModel.Email) ? "Укажите хотя бы телефон или email" : "";
            saveButton.IsEnabled = IsValid();
        }
        private void FocusFirstInvalid()
        {
            if (string.IsNullOrWhiteSpace(ViewModel.FirstName) && string.IsNullOrWhiteSpace(ViewModel.LastName) && string.IsNullOrWhiteSpace(ViewModel.MiddleName))
                firstNameBox.Focus();
            else if (string.IsNullOrWhiteSpace(ViewModel.Phone) && string.IsNullOrWhiteSpace(ViewModel.Email))
                phoneBox.Focus();
        }
        private bool IsValid()
        {
            return (!string.IsNullOrWhiteSpace(ViewModel.Phone) || !string.IsNullOrWhiteSpace(ViewModel.Email)) &&
                   (!string.IsNullOrWhiteSpace(ViewModel.FirstName) || !string.IsNullOrWhiteSpace(ViewModel.LastName) || !string.IsNullOrWhiteSpace(ViewModel.MiddleName));
        }
    }
} 