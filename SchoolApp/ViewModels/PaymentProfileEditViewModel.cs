using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SchoolApp.Models;

namespace SchoolApp.ViewModels
{
    public class PaymentProfileEditViewModel
    {
        public PaymentProfile PaymentProfile { get; set; }
        public List<PaymentRule> PaymentRules { get; set; }
        public PaymentRule CurrentPaymentRule { get; set; }
        public PaymentProfileEditViewModel()
        {
            PaymentRules = new List<PaymentRule>();
        }
    }
}