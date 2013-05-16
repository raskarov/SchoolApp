using SchoolApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolApp.ViewModels
{
    public class UserGroupInstanceEditViewModel
    {
        public List<UserGroupInstance> UserGroupInstances { get; set; }
        public List<UserProfile> Students { get; set; }
    }
}