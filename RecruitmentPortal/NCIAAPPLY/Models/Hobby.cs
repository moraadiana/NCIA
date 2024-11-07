using NCIAAPPLY.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCIAAPPLY.Models
{
    public class Hobby
    {
        public string ApplicantHobby { get; set; }
        public List<ApplicantHobby> ApplicantHobbies { get; set; }
    }
}