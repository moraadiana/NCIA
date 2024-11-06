using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCIASupplier.Models
{
    public class Tenders
    {
        public string No { get; set; }
        public string ReqNo { get; set; }
        public string UnitCost { get; set; }
        public string Amount { get; set; }
        public string TenderNo { get; set; }
        public string Description { get; set; }
        public string ApplicationDate { get; set; }
        public string OpeningDate { get; set; }
        public string ClosingDate { get; set; }
        public string Quantity { get; set; }
        public string Status { get; set; }
        public string UnitofMeasure { get; set; }
        public List<Tenders> openTenders { get; set; }
        public List<Tenders> Lines { get; set; }
        public HttpPostedFileBase TechnicalDocument { get; set; }
        public HttpPostedFileBase FinancialDocument { get; set; }
        public HttpPostedFileBase OtherAttachments { get; set; }
        public HttpPostedFileBase RFQForm { get; set; }
        public string SelectedCategories { get; set; }
    }
}