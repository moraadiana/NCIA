using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCIAVendor.Models
{
    public class Registration
    {
        public string KRAPin { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhone { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string Period { get; set; }
        public string Status { get; set; }
        public string SelectedCategories { get; set; }
        public string Counting { get; set; }
        public string CategoryName { get; set; }
        public List<Registration> Periods { get; set; }
        public HttpPostedFileBase KRACert { get; set; }
        public HttpPostedFileBase IncopCert { get; set; }
        public HttpPostedFileBase CompCert { get; set; }
        public HttpPostedFileBase PrequalificationDocument { get; set; }
    }
}