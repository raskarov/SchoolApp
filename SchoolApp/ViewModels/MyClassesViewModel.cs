using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolApp.ViewModels
{
    public class MyClassesViewModel
    {
        public DateTime Time { get; set; }
        public string GroupName { get; set; }
        public List<string> Students { get; set; }
    }
}