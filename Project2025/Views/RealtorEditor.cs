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
            var panel = new StackPanel
            {
                Margin = new Thickness(15),
                Spacing = 10
            };
            panel.Children.Add(new TextBlock
            {
                Text = "Realtor Details",
                FontSize = 16,
                FontWeight = Avalonia.Media.FontWeight.Bold
            });
            lastNameBox = new TextBox { Watermark = "Last Name" };
            lastNameBox.Bind(TextBox.TextProperty, new Binding("LastName") { Source = _realtor, Mode = BindingMode.TwoWay });
            panel.Children.Add(lastNameBox);
            var lastNameError = new TextBlock
            {
                Text = "Фамилия обязательна",
                Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red),
                IsVisible = string.IsNullOrWhiteSpace(_realtor.LastName)
            };
            panel.Children.Add(lastNameError);
            firstNameBox = new TextBox { Watermark = "First Name" };
            firstNameBox.Bind(TextBox.TextProperty, new Binding("FirstName") { Source = _realtor, Mode = BindingMode.TwoWay });
            panel.Children.Add(firstNameBox);
            var firstNameError = new TextBlock
            {
                Text = "Имя обязательно",
                Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red),
                IsVisible = string.IsNullOrWhiteSpace(_realtor.FirstName)
            };
            panel.Children.Add(firstNameError);
            middleNameBox = new TextBox { Watermark = "Middle Name" };
            middleNameBox.Bind(TextBox.TextProperty, new Binding("MiddleName") { Source = _realtor, Mode = BindingMode.TwoWay });
            panel.Children.Add(middleNameBox);
            var middleNameError = new TextBlock
            {
                Text = "Отчество обязательно",
                Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red),
                IsVisible = string.IsNullOrWhiteSpace(_realtor.MiddleName)
            };
            panel.Children.Add(middleNameError);
            commissionBox = new TextBox { Watermark = "Commission % (0-100)" };
            commissionBox.Bind(TextBox.TextProperty, new Binding("CommissionShare")
            {
                Source = _realtor,
                Mode = BindingMode.TwoWay,
                StringFormat = "{0:0}"
            });
            panel.Children.Add(commissionBox);
            var commissionError = new TextBlock
            {
                Text = "Комиссия должна быть от 0 до 100",
                Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red),
                IsVisible = _realtor.CommissionShare.HasValue && (_realtor.CommissionShare < 0 || _realtor.CommissionShare > 100)
            };
            panel.Children.Add(commissionError);
            var buttonPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 10, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right };
            saveButton = new Button
            {
                Content = "Save",
                Width = 80,
                IsEnabled = IsNameValid() && IsCommissionValid()
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
            deleteButton = new Button
            {
                Content = "Delete",
                Width = 80,
                IsVisible = _realtor.Id != 0
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