using SchoolApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolApp.ViewModels
{
    public class StudentEditViewModel
    {
        public UserProfile Student { get; set; }
        public SelectList LevelsList { get; set; }
        public Level StudentLevel { get; set; }
    }
}