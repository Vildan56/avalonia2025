using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Project2025.Models;

namespace Project2025.Views
{
    public class RealtorEditor : Window
    {
        private readonly Realtor _realtor;
        public RealtorEditor(Realtor realtor)
        {
            _realtor = realtor;
            InitializeComponent();
            DataContext = this;
        }
        private void InitializeComponent()
        {
            Width = 300;
            Height = 250;
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
            var lastNameBox = new TextBox { Watermark = "Last Name" };
            lastNameBox.Bind(TextBox.TextProperty, new Binding("LastName") { Source = _realtor, Mode = BindingMode.TwoWay });
            panel.Children.Add(lastNameBox);
            var firstNameBox = new TextBox { Watermark = "First Name" };
            firstNameBox.Bind(TextBox.TextProperty, new Binding("FirstName") { Source = _realtor, Mode = BindingMode.TwoWay });
            panel.Children.Add(firstNameBox);
            var middleNameBox = new TextBox { Watermark = "Middle Name" };
            middleNameBox.Bind(TextBox.TextProperty, new Binding("MiddleName") { Source = _realtor, Mode = BindingMode.TwoWay });
            panel.Children.Add(middleNameBox);
            var commissionBox = new TextBox { Watermark = "Commission % (0-100)" };
            commissionBox.Bind(TextBox.TextProperty, new Binding("CommissionShare")
            {
                Source = _realtor,
                Mode = BindingMode.TwoWay,
                StringFormat = "{0:0}"
            });
            panel.Children.Add(commissionBox);
            var nameWarning = new TextBlock
            {
                Text = "* Name fields are required",
                Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red),
                IsVisible = !_realtor.IsNameValid
            };
            panel.Children.Add(nameWarning);
            var commissionWarning = new TextBlock
            {
                Text = "* Commission must be 0-100",
                Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red),
                IsVisible = _realtor.CommissionShare.HasValue &&
                           (_realtor.CommissionShare < 0 || _realtor.CommissionShare > 100)
            };
            panel.Children.Add(commissionWarning);
            var saveButton = new Button
            {
                Content = "Save",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
                Width = 80,
                IsEnabled = _realtor.IsNameValid &&
                           (!_realtor.CommissionShare.HasValue ||
                            (_realtor.CommissionShare >= 0 && _realtor.CommissionShare <= 100))
            };
            saveButton.Click += (s, e) => Close(true);
            panel.Children.Add(saveButton);
            Content = panel;
        }
    }
} 