using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Isg.Domain;
namespace SchoolApp.Models
{
    public class PaymentRule : ISoftDelete//, IAuditable
    {
        public int PaymentRuleId { get; set; }

        public string Rule { get; set; }

        /// <summary>
        /// Used as an intermediate column for Rule.
        /// </summary>
        [NotMapped]
        public XElement xRule
        {
            get
            {
                try
                {
                    return XElement.Parse(Rule);
                }
                catch
                {
                    return new XElement("Root");
                }
            }
            set
            {
                Rule = value.ToString();
            }
        }

        [DataType(DataType.Date)]
        public DateTime EffectiveDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

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

        #region Interceptors
        public bool IsDeleted { get; set; }
        //public DateTime CreateDate { get; set; }
        //public string CreateUser { get; set; }
        //public DateTime UpdateDate { get; set; }
        //public string UpdateUser { get; set; }
        #endregion
    }
}