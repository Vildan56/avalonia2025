using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2025
{
    public class Deal
    {
        public int Id { get; set; }
        public int RequirementId { get; set; }
        public int OfferId { get; set; }
        public float CompanyDeduction { get; set; } // FLOAT в БД
        public float BuyerRealtorDeduction { get; set; } // FLOAT в БД
        public float SellerRealtorDeduction { get; set; } // FLOAT в БД
    }
}
