using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolApp.Models;

namespace SchoolApp.ViewModels
{
    public class GroupInstanceDeleteViewModel
    {
        public GroupInstance GroupInstance { get; set; }
        public String StartDate { get; set; }
        public String EndDate { get; set; }
    }
}