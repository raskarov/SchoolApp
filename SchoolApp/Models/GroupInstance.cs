using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using SchoolApp.Models;

namespace SchoolApp.Models
{
    public class GroupInstance
    {
        public int GroupInstanceId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public int GroupId { get; set; }
        public int? ClassroomId { get; set; }

        [ForeignKey("GroupId")]
        public Group Group { get; set; }

        [ForeignKey("ClassroomId")]
        public Classroom Classroom { get; set; }

        public string RecurrenceRule { get; set; }
    }
}