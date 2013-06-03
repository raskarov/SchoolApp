using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using SchoolApp.Models;

namespace SchoolApp.ViewModels
{
    public class GroupIndexViewModel
    {
        public int GroupId { get; set; }
        public string Name { get; set; }

        public List<UserProfile> Teachers { get; set; }
        public List<UserProfile> Students { get; set; }

        public GroupIndexViewModel(Group group)
        {
            this.GroupId = group.GroupId;
            this.Name = group.Name;
            this.Teachers = group.Users.Where(x=>Roles.IsUserInRole(x.UserName, "Teacher")).ToList();
            this.Students = group.Users.Where(x => Roles.IsUserInRole(x.UserName,"Student") && x.FutureStudent==false).ToList(); 
        }
    }
}