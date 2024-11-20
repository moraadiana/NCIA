using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCIAVendor.Models
{
    public class Categories
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string Mandatory { get; set; }
        public string Attached { get; set; }
        public string DocName { get; set; }
        public List<Categories> AllCategories { get; set; }
        public List<Categories> AppliedCategories { get; set; }
        public HttpPostedFileBase DocFile { get; set; }
    }
}