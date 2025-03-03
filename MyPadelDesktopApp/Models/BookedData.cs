using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelDesktopApp.Models
{
    public class BookedData
    {
        public int? id { get; set; }
        public DateTime? date { get; set; }
        public DateTime? endTime { get; set; }
        public string? sportType { get; set; }
        public string? fieldName { get; set; }
        public int? fieldId { get; set; }
        public string? paymentMethod { get; set; }
        public double? amount { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public int? duration { get; set; }
        public string? status { get; set; }
    }
}
