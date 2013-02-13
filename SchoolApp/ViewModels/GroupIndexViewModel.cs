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

        public List<string> Teachers { get; set; }
        public List<string> Students { get; set; }

        public GroupIndexViewModel(Group group)
        {
            this.GroupId = group.GroupId;
            this.Name = group.Name;
            this.Teachers = group.Users.Where(x=>Roles.IsUserInRole(x.UserName, "Teacher")).Select(x => x.FullName).ToList();
            this.Students = group.Users.Where(x => Roles.IsUserInRole(x.UserName,"Student")).Select(x => x.FullName).ToList(); 
        }
    }
}