using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Isg.Domain;
namespace SchoolApp.Models
{
    public class Guardian : ISoftDelete, IAuditable
    {
        public int GuardianId { get; set; }

        [Display(Name="Имя")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        public string Email { get; set; }

        [Display(Name = "Телефон")]
        public string Phone { get; set; }

        [Display(Name = "Кем приходится")]
        public string Relationship { get; set; }

        #region Interceptors
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        #endregion
    }
}