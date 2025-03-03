using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelDesktopApp.Models
{
    public class CustomerModel
    {
        public string ClientName { get; set; } = "";
        public string CustomerSurname { get; set; } = "";
        public DateTime? FitCardExpiryDate { get; set; }
        public string Notes { get; set; } = "";
        public string Email { get; set; } = "";
        public string Telephone { get; set; } = "";
        public string PlayerType { get; set; } = "";
        public string Tag { get; set; } = "";
        public bool MarketingInformationEnabled { get; set; } = false;
    }
}
