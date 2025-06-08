using HarfBuzzSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2025
{
    public class PropertyObject
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Apartment { get; set; }
        public float Latitude { get; set; } // FLOAT в БД
        public float Longitude { get; set; } // FLOAT в БД
        public int Floor { get; set; }
        public int RoomsCount { get; set; }
        public float Area { get; set; } // FLOAT в БД
        public int PropertyTypeId { get; set; }
        public int DistrictId { get; set; }

        // Навигационное свойство для EF Core
        public PropertyType PropertyType { get; set; }
    }
}
