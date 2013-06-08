using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SchoolApp.Models;

namespace SchoolApp.ViewModels
{
    public class StudentCardViewModel
    {
        public int UserId { get; set; }
        public string FullName {get;set;}
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Level { get; set; }
    }
}