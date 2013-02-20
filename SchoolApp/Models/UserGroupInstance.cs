using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SchoolApp.Models
{
    public enum AttendanceType
    {
        Present,
        Absent,
        AbsentWithExcuse
    }
    public class UserGroupInstance
    {
        public int UserGroupInstanceID { get; set; }

        public AttendanceType Present { get; set; }
        public DateTime AttendanceTaken { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public UserProfile User { get; set; }

        public int GroupInstanceId { get; set; }

        [ForeignKey("GroupInstanceId")]
        public GroupInstance GroupInstance { get; set; }
    }
}