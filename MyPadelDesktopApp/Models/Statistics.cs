using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelDesktopApp.Models
{
    public class Statistics
    {
        public int? totalBookings { get; set; } = 0;
        public int? averageMonthlyBookings { get; set; } = 0;
        public string? favoriteField { get; set; }
        public int? cancelledBookings { get; set; } = 0;
        public double? lifetimeValueInEuros { get; set; } = 0;
    }
}
