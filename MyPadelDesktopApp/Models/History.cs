using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelDesktopApp.Models
{
    public class History
    {
        public DateTime? date { get; set; }
        public string? field { get; set; }
        public DateTime? startingHour { get; set; }
        public string? duration { get; set; }
        public double? amount { get; set; }
        public string? paymentMethod { get; set; }
    }
}
