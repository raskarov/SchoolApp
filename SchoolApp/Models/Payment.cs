using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Isg.Domain;
namespace SchoolApp.Models
{
    public class Payment : ISoftDelete//, IAuditable
    {
        public int PaymentId { get; set; }

        [Display(Name=@"Дата\время транзакции")]
        public DateTime TransactionDateTime { get; set; }

        [Display(Name="Сумма")]
        public decimal Amount { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public UserProfile Payee { get; set; }
        [Display(Name="Комментарии")]
        public string comments { get; set; }

        #region Interceptors
        public bool IsDeleted { get; set; }
        //public DateTime CreateDate { get; set; }
        //public string CreateUser { get; set; }
        //public DateTime UpdateDate { get; set; }
        //public string UpdateUser { get; set; }
        #endregion
    }
}