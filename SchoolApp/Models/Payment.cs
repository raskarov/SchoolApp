using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SchoolApp.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        public DateTime TransactionDateTime { get; set; }

        public decimal Amount { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public UserProfile Payee { get; set; }

        public string comments { get; set; }
    }
}