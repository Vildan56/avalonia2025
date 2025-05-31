using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using Project2025.Models;

namespace Project2025.ViewModels
{
    public class ClientViewModel : ReactiveObject
    {
        public ObservableCollection<Client> Clients { get; } = new();

        private Client? _selectedClient;
        public Client? SelectedClient
        {
            get => _selectedClient;
            set => this.RaiseAndSetIfChanged(ref _selectedClient, value);
        }

        public bool HasSelectedClient => SelectedClient != null;
        public bool CanDeleteClient => HasSelectedClient && !SelectedClient.HasNeedsOrOffers;

        public ReactiveCommand<Unit, Unit> AddClientCommand { get; }
        public ReactiveCommand<Unit, Unit> EditClientCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteClientCommand { get; }

        public ClientViewModel()
        {
            AddClientCommand = ReactiveCommand.Create(AddClient);
            EditClientCommand = ReactiveCommand.Create(EditClient,
                this.WhenAnyValue(vm => vm.HasSelectedClient));
            DeleteClientCommand = ReactiveCommand.Create(DeleteClient,
                this.WhenAnyValue(vm => vm.CanDeleteClient));

            Clients.Add(new Client
            {
                Id = 1,
                LastName = "Ivanov",
                FirstName = "Ivan",
                Phone = "+7 (999) 123-4567"
            });
        }

        private void AddClient() => ShowEditor(new Client());
        private void EditClient() => ShowEditor(SelectedClient!);

        private async void ShowEditor(Client client)
        {
            var editor = new Project2025.Views.ClientEditor(client);

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (await editor.ShowDialog<bool>(desktop.MainWindow))
                {
                    if (client.Id == 0)
                    {
                        client.Id = Clients.Count + 1;
                        Clients.Add(client);
                    }
                }
            }
        }

        private void DeleteClient()
        {
            if (SelectedClient != null) Clients.Remove(SelectedClient);
        }
    }
} 