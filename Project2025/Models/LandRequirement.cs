using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2025
{
    public class LandRequirement
    {
        public int Id { get; set; }
        public float MinArea { get; set; } // FLOAT в БД
        public float MaxArea { get; set; } // FLOAT в БД
        public int RequirementId { get; set; }

    }
}
