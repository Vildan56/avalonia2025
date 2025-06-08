using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Layout;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia.ReactiveUI;
using Avalonia.Controls.ApplicationLifetimes;
using Project2025.ViewModels;

namespace Project2025.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void RealEstateGrid_DoubleTapped(object? sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm && vm.RealEstateVM.HasSelectedProperty)
            {
                vm.RealEstateVM.EditPropertyCommand.Execute().Subscribe();
            }
        }

        private void RealtorGrid_DoubleTapped(object? sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm && vm.RealtorVM.HasSelectedRealtor)
            {
                vm.RealtorVM.EditRealtorCommand.Execute().Subscribe();
            }
        }

        private void ClientGrid_DoubleTapped(object? sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm && vm.ClientVM.HasSelectedClient)
            {
                vm.ClientVM.EditClientCommand.Execute().Subscribe();
            }
        }
    }
}