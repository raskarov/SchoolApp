using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Isg.Domain;
namespace SchoolApp.Models
{
    public class Classroom : ISoftDelete//, IAuditable
    {
        public int ClassroomID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Location { get; set; }

        public string Comments { get; set; }

        #region Interceptors
        public bool IsDeleted { get; set; }
        //public DateTime CreateDate { get; set; }
        //public string CreateUser { get; set; }
        //public DateTime UpdateDate { get; set; }
        //public string UpdateUser { get; set; }
        #endregion
    }
}