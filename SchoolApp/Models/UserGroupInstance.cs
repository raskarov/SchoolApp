using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Isg.Domain;
using System.ComponentModel;
namespace SchoolApp.Models
{
    public enum AttendanceType
    {
        [Description("NA")]
        NA,
        [Description("Присутствовал")]
        Present,
        [Description("Отсутствовал")]
        Absent,
        [Description("Отсутствовал и предупредил")]
        AbsentWithExcuse
    }
    public class UserGroupInstance : ISoftDelete//, IAuditable
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

        /// <summary>
        /// The actual date for the attendace if recurrence is not null
        /// </summary>
        public DateTime InstanceDateTime { get; set; }

        #region Interceptors
        public bool IsDeleted { get; set; }
        //public DateTime CreateDate { get; set; }
        //public string CreateUser { get; set; }
        //public DateTime UpdateDate { get; set; }
        //public string UpdateUser { get; set; }
        #endregion
    }
}