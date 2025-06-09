using ReactiveUI;

namespace Project2025.Models
{
    public class Client : ReactiveObject
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => this.RaiseAndSetIfChanged(ref _id, value);
        }

        private string? _lastName;
        public string? LastName
        {
            get => _lastName;
            set => this.RaiseAndSetIfChanged(ref _lastName, value);
        }

        private string? _firstName;
        public string? FirstName
        {
            get => _firstName;
            set => this.RaiseAndSetIfChanged(ref _firstName, value);
        }

        private string? _middleName;
        public string? MiddleName
        {
            get => _middleName;
            set => this.RaiseAndSetIfChanged(ref _middleName, value);
        }

        private string? _phone;
        public string? Phone
        {
            get => _phone;
            set => this.RaiseAndSetIfChanged(ref _phone, value);
        }

        private string? _email;
        public string? Email
        {
            get => _email;
            set => this.RaiseAndSetIfChanged(ref _email, value);
        }

        private bool _hasNeedsOrOffers;
        public bool HasNeedsOrOffers
        {
            get => _hasNeedsOrOffers;
            set => this.RaiseAndSetIfChanged(ref _hasNeedsOrOffers, value);
        }

        public bool HasContactInfo => !string.IsNullOrWhiteSpace(Phone) || !string.IsNullOrWhiteSpace(Email);
        public string FullName => $"{LastName} {FirstName} {MiddleName}".Trim();
    }
}
