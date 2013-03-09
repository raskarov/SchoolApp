using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolApp.Models;

namespace SchoolApp.ViewModels
{
    public class UserIndexViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public bool IsRegistered { get; set; }
        public bool IsTeacher { get; set; }
        public bool IsAdmin { get; set; }
    }
}