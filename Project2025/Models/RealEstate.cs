using System.Linq;
using ReactiveUI;

namespace Project2025.Models
{
    public class RealEstate : ReactiveObject
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => this.RaiseAndSetIfChanged(ref _id, value);
        }

        private string _type = string.Empty;
        public string Type
        {
            get => _type;
            set => this.RaiseAndSetIfChanged(ref _type, value);
        }

        private Address _address = new();
        public Address Address
        {
            get => _address;
            set => this.RaiseAndSetIfChanged(ref _address, value);
        }

        private Coordinates _coordinates = new();
        public Coordinates Coordinates
        {
            get => _coordinates;
            set => this.RaiseAndSetIfChanged(ref _coordinates, value);
        }

        private bool _hasOffers;
        public bool HasOffers
        {
            get => _hasOffers;
            set => this.RaiseAndSetIfChanged(ref _hasOffers, value);
        }

        public string FullAddress => Address.ToString();
        public string CoordString => Coordinates != null
            ? $"{Coordinates.Latitude:0.#####}, {Coordinates.Longitude:0.#####}"
            : "No coordinates";
    }

    public class Address : ReactiveObject
    {
        private string _city = string.Empty;
        public string City
        {
            get => _city;
            set => this.RaiseAndSetIfChanged(ref _city, value);
        }

        private string _street = string.Empty;
        public string Street
        {
            get => _street;
            set => this.RaiseAndSetIfChanged(ref _street, value);
        }

        private string? _houseNumber;
        public string? HouseNumber
        {
            get => _houseNumber;
            set => this.RaiseAndSetIfChanged(ref _houseNumber, value);
        }

        private string? _apartmentNumber;
        public string? ApartmentNumber
        {
            get => _apartmentNumber;
            set => this.RaiseAndSetIfChanged(ref _apartmentNumber, value);
        }

        public override string ToString()
        {
            var parts = new[] { City, Street, HouseNumber, ApartmentNumber }
                .Where(p => !string.IsNullOrWhiteSpace(p));
            return string.Join(", ", parts);
        }
    }

    public class Coordinates : ReactiveObject
    {
        private double _latitude;
        public double Latitude
        {
            get => _latitude;
            set => this.RaiseAndSetIfChanged(ref _latitude, value);
        }

        private double _longitude;
        public double Longitude
        {
            get => _longitude;
            set => this.RaiseAndSetIfChanged(ref _longitude, value);
        }
    }
} 