using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCIASupplier.Models
{
    public class BidderDetails
    {
        public string BidderNo { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string ContactPerson { get; set; }
        public string CompanyContact { get; set; }
        public string ContactPersonEmail { get; set; }
        public string CompanyEmail { get; set; }
        public string VatRegistrationNo { get; set; }
        public string COI { get; set; }
        public string PRC { get; set; }
        public string VRC { get; set; }
        public string TCC { get; set; }
    }
}