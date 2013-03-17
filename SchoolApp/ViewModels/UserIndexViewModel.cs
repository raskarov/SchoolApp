using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolApp.Models;
using System.ComponentModel.DataAnnotations;

namespace SchoolApp.ViewModels
{
    public class UserIndexViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        [Display(Name="Имя")]
        public string FullName { get; set; }

        public bool IsRegistered { get; set; }
        
        [Display(Name="Учитель?")]
        public bool IsTeacher { get; set; }

        [Display(Name="Администратор?")]
        public bool IsAdmin { get; set; }
    }
}