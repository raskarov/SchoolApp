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
        public List<PaymentRule> OldPaymentRules { get; set; }
        public PaymentRule FuturePaymentRule { get; set; }
        public PaymentRule CurrentPaymentRule { get; set; }
        public PaymentProfileEditViewModel()
        {
            OldPaymentRules = new List<PaymentRule>();
            FuturePaymentRule = new PaymentRule();
            CurrentPaymentRule = new PaymentRule();
        }
    }
}