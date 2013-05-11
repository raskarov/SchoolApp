using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Security;
using SchoolApp.Models;
using Isg.Domain;
namespace SchoolApp.Models
{
    public class Group : ISoftDelete//, IAuditable
    {
        public int GroupId { get; set; }

        public string Name { get; set; }

        public int? PaymentProfileId { get; set; }
        
        [ForeignKey("PaymentProfileId")]
        public virtual PaymentProfile PaymentProfile { get; set; }

        public ICollection<UserProfile> Users { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual Group ParentGroup { get; set; }
        #region Interceptors
        public bool IsDeleted { get; set; }
        //public DateTime CreateDate { get; set; }
        //public string CreateUser { get; set; }
        //public DateTime UpdateDate { get; set; }
        //public string UpdateUser { get; set; }
        #endregion
    }
}