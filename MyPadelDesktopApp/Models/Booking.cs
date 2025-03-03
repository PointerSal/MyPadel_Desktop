using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelDesktopApp.Models
{
    public class Booking
    {
        public int? id { get; set; }
        public string? sportsName { get; set; }
        public string? fieldName { get; set; }
        public string? fieldType { get; set; }
        public string? terrainType { get; set; }
        public int? fieldCapacity { get; set; }
        public int? slot1Duration { get; set; } = 0;
        public double? slot1Price { get; set; } = 0;
        public int? slot2Duration { get; set; } = 0;
        public double? slot2Price { get; set; } = 0;
        public int? slot3Duration { get; set; } = 0;
        public double? slot3Price { get; set; } = 0;
        public bool canBeBooked { get; set; } = false;
        public string? openingHours { get; set; }
    }
}
