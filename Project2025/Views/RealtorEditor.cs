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
    public class RealtorEditor : Window
    {
        private readonly Realtor _realtor;
        private TextBox lastNameBox, firstNameBox, middleNameBox, commissionBox;
        private Button saveButton, deleteButton;
        public RealtorEditor(Realtor realtor)
        {
            _realtor = realtor;
            InitializeComponent();
            DataContext = this;
        }
        private void InitializeComponent()
        {
            Width = 300;
            Height = 350;
            Title = "Realtor Editor";
            var mainPanel = new StackPanel
            {
                Background = Avalonia.Media.Brushes.White,
                Margin = new Thickness(15),
                Spacing = 10
            };
            // Заголовок с логотипом
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
                Text = "Добавление/редактирование риэлтора",
                FontSize = 20,
                FontWeight = Avalonia.Media.FontWeight.Bold,
                Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("#004AFF")),
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 20)
            });
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
            lastNameBox.Bind(TextBox.TextProperty, new Binding("LastName") { Source = _realtor, Mode = BindingMode.TwoWay });
            mainPanel.Children.Add(lastNameBox);
            var lastNameError = new TextBlock
            {
                Text = "Фамилия обязательна",
                Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red),
                IsVisible = string.IsNullOrWhiteSpace(_realtor.LastName)
            };
            mainPanel.Children.Add(lastNameError);
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
            firstNameBox.Bind(TextBox.TextProperty, new Binding("FirstName") { Source = _realtor, Mode = BindingMode.TwoWay });
            mainPanel.Children.Add(firstNameBox);
            var firstNameError = new TextBlock
            {
                Text = "Имя обязательно",
                Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red),
                IsVisible = string.IsNullOrWhiteSpace(_realtor.FirstName)
            };
            mainPanel.Children.Add(firstNameError);
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
            middleNameBox.Bind(TextBox.TextProperty, new Binding("MiddleName") { Source = _realtor, Mode = BindingMode.TwoWay });
            mainPanel.Children.Add(middleNameBox);
            var middleNameError = new TextBlock
            {
                Text = "Отчество обязательно",
                Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red),
                IsVisible = string.IsNullOrWhiteSpace(_realtor.MiddleName)
            };
            mainPanel.Children.Add(middleNameError);
            commissionBox = new TextBox {
                Watermark = "Commission % (0-100)",
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
            commissionBox.Bind(TextBox.TextProperty, new Binding("CommissionShare")
            {
                Source = _realtor,
                Mode = BindingMode.TwoWay,
                StringFormat = "{0:0}"
            });
            mainPanel.Children.Add(commissionBox);
            var commissionError = new TextBlock
            {
                Text = "Комиссия должна быть от 0 до 100",
                Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red),
                IsVisible = _realtor.CommissionShare.HasValue && (_realtor.CommissionShare < 0 || _realtor.CommissionShare > 100)
            };
            mainPanel.Children.Add(commissionError);
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
                if (IsNameValid() && IsCommissionValid())
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

            void UpdateValidation(object? sender, EventArgs? e)
            {
                saveButton.IsEnabled = IsNameValid() && IsCommissionValid();
                lastNameError.IsVisible = string.IsNullOrWhiteSpace(_realtor.LastName);
                firstNameError.IsVisible = string.IsNullOrWhiteSpace(_realtor.FirstName);
                middleNameError.IsVisible = string.IsNullOrWhiteSpace(_realtor.MiddleName);
                commissionError.IsVisible = _realtor.CommissionShare.HasValue && (_realtor.CommissionShare < 0 || _realtor.CommissionShare > 100);
            }
            lastNameBox.PropertyChanged += UpdateValidation;
            firstNameBox.PropertyChanged += UpdateValidation;
            middleNameBox.PropertyChanged += UpdateValidation;
            commissionBox.PropertyChanged += UpdateValidation;
            // Автофокус при открытии
            FocusFirstInvalid();
        }
        private void FocusFirstInvalid()
        {
            if (string.IsNullOrWhiteSpace(_realtor.LastName))
                lastNameBox.Focus();
            else if (string.IsNullOrWhiteSpace(_realtor.FirstName))
                firstNameBox.Focus();
            else if (string.IsNullOrWhiteSpace(_realtor.MiddleName))
                middleNameBox.Focus();
            else if (_realtor.CommissionShare.HasValue && (_realtor.CommissionShare < 0 || _realtor.CommissionShare > 100))
                commissionBox.Focus();
        }
        private bool IsNameValid()
        {
            return !string.IsNullOrWhiteSpace(_realtor.LastName) &&
                   !string.IsNullOrWhiteSpace(_realtor.FirstName) &&
                   !string.IsNullOrWhiteSpace(_realtor.MiddleName);
        }
        private bool IsCommissionValid()
        {
            return !_realtor.CommissionShare.HasValue || (_realtor.CommissionShare >= 0 && _realtor.CommissionShare <= 100);
        }
    }
} 