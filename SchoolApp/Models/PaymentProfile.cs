using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolApp.Models
{
    public class PaymentProfile
    {
        public int PaymentProfileId { get; set; }
        public string Name { get; set; }
        public string Comments { get; set; }
    }
}