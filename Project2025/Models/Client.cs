using ReactiveUI;
using ReactiveValidation;
using ReactiveValidation.Extensions;

namespace Project2025.Models
{
    public class Client : ReactiveValidatableObject
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

        private string _phone = string.Empty;
        public string Phone
        {
            get => _phone;
            set => this.RaiseAndSetIfChanged(ref _phone, value);
        }

        private string _email = string.Empty;
        public string Email
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

        public Client()
        {
            Validator = GetValidator();
        }

        private IObjectValidator GetValidator()
        {
            var builder = new ValidationBuilder<Client>();
            builder.RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Имя обязательно");
            builder.RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Фамилия обязательна");
            builder.RuleFor(x => x.Phone)
                .Matches(@"^\+?\d{10,15}$").WithMessage("Некорректный телефон")
                .When(x => !string.IsNullOrWhiteSpace(x.Phone));
            builder.RuleFor(x => x.Email)
                .Email().WithMessage("Некорректный email")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));
            builder.RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.Phone) || !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage("Укажите хотя бы телефон или email");
            return builder.Build(this);
        }
    }
}
