using ReactiveUI;
using ReactiveValidation;
using ReactiveValidation.Extensions;

namespace Project2025.Models
{
    public class Realtor : ReactiveValidatableObject
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

        public Realtor()
        {
            Validator = GetValidator();
        }

        private IObjectValidator GetValidator()
        {
            var builder = new ValidationBuilder<Realtor>();
            builder.RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Фамилия обязательна");
            builder.RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Имя обязательно");
            builder.RuleFor(x => x.MiddleName)
                .NotEmpty().WithMessage("Отчество обязательно");
            builder.RuleFor(x => x.CommissionShare)
                .InclusiveBetween(0, 100).WithMessage("Комиссия должна быть от 0 до 100")
                .When(x => x.CommissionShare.HasValue);
            return builder.Build(this);
        }
    }
}
