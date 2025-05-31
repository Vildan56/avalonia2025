using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2025
{
    public class ApartmentRequirement
    {
        public int Id { get; set; }
        public int MinFloor { get; set; }
        public int MaxFloor { get; set; }
        public float MinArea { get; set; } // FLOAT в БД
        public float MaxArea { get; set; } // FLOAT в БД
        public int MinRooms { get; set; }
        public int MaxRooms { get; set; }
        public int RequirementId { get; set; }

    }
}
