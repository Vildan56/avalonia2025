using ReactiveUI;

namespace Project2025.Models
{
    public class Realtor : ReactiveObject
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => this.RaiseAndSetIfChanged(ref _id, value);
        }

        private string _lastName = string.Empty;
        public string LastName
        {
            get => _lastName;
            set => this.RaiseAndSetIfChanged(ref _lastName, value);
        }

        private string _firstName = string.Empty;
        public string FirstName
        {
            get => _firstName;
            set => this.RaiseAndSetIfChanged(ref _firstName, value);
        }

        private string _middleName = string.Empty;
        public string MiddleName
        {
            get => _middleName;
            set => this.RaiseAndSetIfChanged(ref _middleName, value);
        }

        private double? _commissionShare;
        public double? CommissionShare
        {
            get => _commissionShare;
            set => this.RaiseAndSetIfChanged(ref _commissionShare, value);
        }

        private bool _hasNeedsOrOffers;
        public bool HasNeedsOrOffers
        {
            get => _hasNeedsOrOffers;
            set => this.RaiseAndSetIfChanged(ref _hasNeedsOrOffers, value);
        }

        public bool IsNameValid =>
            !string.IsNullOrWhiteSpace(LastName) &&
            !string.IsNullOrWhiteSpace(FirstName) &&
            !string.IsNullOrWhiteSpace(MiddleName);

        public string FullName => $"{LastName} {FirstName} {MiddleName}";
        public string CommissionDisplay => CommissionShare.HasValue ? $"{CommissionShare}%" : "N/A";
    }
}
