using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2025
{
    public class Requirement
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public int MinPrice { get; set; } // INT в БД
        public int MaxPrice { get; set; } // INT в БД
        public string Apartment { get; set; }
        public int ClientId { get; set; }
        public int? RealtorId { get; set; }
        public int PropertyTypeId { get; set; }
        public int DistrictId { get; set; }

    }
}
