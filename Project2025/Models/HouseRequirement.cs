using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2025
{
    public class HouseRequirement
    {
        public int Id { get; set; }
        public int MinFloors { get; set; }
        public int MaxFloors { get; set; }
        public float MinArea { get; set; } // FLOAT в БД
        public float MaxArea { get; set; } // FLOAT в БД
        public int MinRooms { get; set; }
        public int MaxRooms { get; set; }
        public int RequirementId { get; set; }

    }
}
