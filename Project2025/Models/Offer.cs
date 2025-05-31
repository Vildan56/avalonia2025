using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2025
{
    public class Offer
    {
        public int Id { get; set; }
        public int Price { get; set; } // INT в БД
        public int ClientId { get; set; }
        public int RealtorId { get; set; }
        public int PropertyObjectId { get; set; }
    }
}
