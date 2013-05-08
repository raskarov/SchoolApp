using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Isg.Domain;
namespace SchoolApp.Models
{
    public class PaymentProfile : ISoftDelete//, IAuditable
    {
        public int PaymentProfileId { get; set; }
        public string Name { get; set; }
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