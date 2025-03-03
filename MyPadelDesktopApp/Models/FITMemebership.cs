using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelDesktopApp.Models
{
    public class FITMemebership
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public DateTime FitCardExpiryDate { get; set; } = DateTime.Now;
        public DateTime CertificateExpiryDate { get; set; } = DateTime.Now;
    }
}
