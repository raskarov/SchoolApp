using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using SchoolApp.Models;

namespace SchoolApp.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public string Name { get; set; }
        public ICollection<UserProfile> Users { get; set; }
    }
}