using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SchoolApp.Models
{
    public class Payment
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
    }
}