using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolApp.Models
{
    public class Classroom
    {
        public int ClassroomID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Location { get; set; }

        public string Comments { get; set; }
    }
}