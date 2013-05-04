using SchoolApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolApp.ViewModels
{
    public class StudentIndexVieWModel
    {
        public UserProfile Student { get; set; }
        public String GuardianPhone { get; set; }
        public String GuardianName { get; set; }
    }
}