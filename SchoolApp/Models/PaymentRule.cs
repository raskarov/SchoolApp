using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SchoolApp.Models
{
    public class PaymentRule
    {
        public int PaymentRuleId { get; set; }

        public string Rule { get; set; }

        [DataType(DataType.Date)]
        public DateTime EffectiveDate { get; set; }

        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        /// <summary>
        /// Variable rate is the rate that depends on number of users.
        /// If Variable = false, Rule is not used.
        /// </summary>
        public bool Variable { get; set; }

        public int PaymentProfileId { get; set; }

        [ForeignKey("PaymentProfileId")]
        public PaymentProfile PaymentProfile { get; set; }

        public PaymentRule()
        {
            Variable = false;
        }
    }
}