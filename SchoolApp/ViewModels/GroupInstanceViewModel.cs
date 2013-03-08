using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolApp.Models;

namespace SchoolApp.ViewModels
{
    public class GroupInstanceViewModel
    {
        public List<GroupInstance> GroupInstances { get; set; }
        public List<SelectListItem> TeachersList { get; set; }
        public Dictionary<string, string> ColorLegend { get; set; } //Will fail if two teachers have the same name :)
    }
}