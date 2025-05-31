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

        private string _type = "";
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

        private Coordinates? _coordinates;
        public Coordinates? Coordinates
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
        public string CoordString => Coordinates.HasValue ?
            $"{Coordinates.Value.Latitude:0.#####}, {Coordinates.Value.Longitude:0.#####}" : "No coordinates";
    }

    public class Address : ReactiveObject
    {
        private string? _city;
        public string? City
        {
            get => _city;
            set => this.RaiseAndSetIfChanged(ref _city, value);
        }

        private string? _street;
        public string? Street
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

    public struct Coordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public bool IsValid =>
            Latitude >= -90 && Latitude <= 90 &&
            Longitude >= -180 && Longitude <= 180;
    }
} 