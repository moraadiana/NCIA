using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCIAAPPLY.ViewModels
{
    public class ApplicantQualification
    {
        public int Counter { get; set; }
        public string ApplitionNo { get; set; }
        public string QualificationType { get; set; }
        public string QualificationCode { get; set; }
        public string QualificationDescription { get; set; }
        public string Institution { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}