using SchoolApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolApp.ViewModels
{
    public class StudentPhotoViewModel
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public HttpPostedFileBase Photo { get; set; }
    }
}