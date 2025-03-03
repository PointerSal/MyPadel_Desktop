using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelDesktopApp.Models
{
    public class UsersData
    {
        public string? clientName { get; set; }
        public string? customerSurname { get; set; }
        public string? email { get; set; }
        public string? telephone { get; set; }
        public DateTime? fitCardExpiryDate { get; set; }
        public DateTime? medicalCertificateExpiryDate { get; set; }
        public string? playerType { get; set; }
    }
}
