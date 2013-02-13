using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolApp.Models;

namespace SchoolApp.ViewModels
{
    public class GroupEditViewModel
    {
        public Group Group { get; set; }
        public int[] SelectedTeacherIds { get; set; }
        public MultiSelectList Teachers { get; set; }
        public int[] SelectedStudentIds { get; set; }
        public MultiSelectList Students { get; set; }
    }
}