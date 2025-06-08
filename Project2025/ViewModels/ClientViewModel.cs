using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using Project2025.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Project2025.ViewModels
{
    public class ClientViewModel : ReactiveObject
    {
        public ObservableCollection<Client> Clients { get; } = new();
        public ObservableCollection<Client> FilteredClients { get; } = new();

        private Client? _selectedClient;
        public Client? SelectedClient
        {
            get => _selectedClient;
            set => this.RaiseAndSetIfChanged(ref _selectedClient, value);
        }

        public bool HasSelectedClient => SelectedClient != null;
        public bool CanDeleteClient => HasSelectedClient && !SelectedClient.HasNeedsOrOffers;

        private string? _fioSearch;
        public string? FioSearch
        {
            get => _fioSearch;
            set => this.RaiseAndSetIfChanged(ref _fioSearch, value);
        }

        public ReactiveCommand<Unit, Unit> AddClientCommand { get; }
        public ReactiveCommand<Unit, Unit> EditClientCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteClientCommand { get; }
        public ReactiveCommand<Unit, Unit> SearchClientsCommand { get; }

        public ClientViewModel()
        {
            AddClientCommand = ReactiveCommand.Create(AddClient);
            EditClientCommand = ReactiveCommand.Create(EditClient,
                this.WhenAnyValue(vm => vm.HasSelectedClient));
            DeleteClientCommand = ReactiveCommand.Create(DeleteClient,
                this.WhenAnyValue(vm => vm.CanDeleteClient));
            SearchClientsCommand = ReactiveCommand.Create(SearchClients);

            // Загружаем данные асинхронно
            LoadClientsAsync();
        }

        private async void LoadClientsAsync()
        {
            using (var db = new Project2025.AppDbContext())
            {
                var clients = await db.Clients.AsNoTracking().ToListAsync();
                
                // Обновляем коллекцию в UI потоке
                await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
                {
                    Clients.Clear();
                    foreach (var client in clients)
                    {
                        Clients.Add(client);
                    }
                    UpdateFilteredClients();
                });
            }
        }

        private void AddClient() => ShowEditor(new Client());
        private void EditClient() => ShowEditor(SelectedClient!);
        
        private async void DeleteClient()
        {
            if (SelectedClient != null)
            {
                using (var db = new Project2025.AppDbContext())
                {
                    var dbObj = await db.Clients.FindAsync(SelectedClient.Id);
                    if (dbObj != null)
                    {
                        db.Clients.Remove(dbObj);
                        await db.SaveChangesAsync();
                    }
                }

                Clients.Remove(SelectedClient);
                UpdateFilteredClients();
            }
        }

        private async void ShowEditor(Client client)
        {
            var editor = new Project2025.Views.ClientEditor(client);

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var result = await editor.ShowDialog<string>(desktop.MainWindow);
                if (result == "save")
                {
                    using (var db = new Project2025.AppDbContext())
                    {
                        if (client.Id == 0) // New client
                        {
                            db.Clients.Add(client);
                            await db.SaveChangesAsync();
                            Clients.Add(client);
                        }
                        else // Existing client
                        {
                            var dbObj = await db.Clients.FindAsync(client.Id);
                            if (dbObj != null)
                            {
                                db.Entry(dbObj).CurrentValues.SetValues(client);
                                await db.SaveChangesAsync();
                                var index = Clients.IndexOf(Clients.First(c => c.Id == client.Id));
                                if (index >= 0)
                                {
                                    Clients[index] = client;
                                }
                            }
                        }
                    }
                    UpdateFilteredClients();
                    SelectedClient = client;
                }
                else if (result == "delete")
                {
                    if (client.Id != 0)
                    {
                        DeleteClient();
                    }
                }
            }
        }

        private void SearchClients() => UpdateFilteredClients();
        
        private void UpdateFilteredClients()
        {
            var currentSelectedId = SelectedClient?.Id;
            
            FilteredClients.Clear();
            var filtered = Clients.AsEnumerable();
            
            if (!string.IsNullOrWhiteSpace(FioSearch))
            {
                filtered = filtered.Where(c => 
                    LevenshteinDistance((c.FullName ?? "").ToLower(), FioSearch.ToLower()) <= 3);
            }
            
            foreach (var item in filtered)
                FilteredClients.Add(item);
            
            // Восстанавливаем выделение
            if (currentSelectedId.HasValue)
            {
                SelectedClient = FilteredClients.FirstOrDefault(c => c.Id == currentSelectedId.Value);
            }
        }
        private int LevenshteinDistance(string s, string t)
        {
            if (string.IsNullOrEmpty(s)) return string.IsNullOrEmpty(t) ? 0 : t.Length;
            if (string.IsNullOrEmpty(t)) return s.Length;
            var d = new int[s.Length + 1, t.Length + 1];
            for (int i = 0; i <= s.Length; i++) d[i, 0] = i;
            for (int j = 0; j <= t.Length; j++) d[0, j] = j;
            for (int i = 1; i <= s.Length; i++)
                for (int j = 1; j <= t.Length; j++)
                {
                    int cost = s[i - 1] == t[j - 1] ? 0 : 1;
                    d[i, j] = new[]
                    {
                        d[i - 1, j] + 1,
                        d[i, j - 1] + 1,
                        d[i - 1, j - 1] + cost
                    }.Min();
                }
            return d[s.Length, t.Length];
        }
        
    }
} 