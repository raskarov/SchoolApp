using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            this.Teachers = group.Users.Select(x => x.FullName).ToList(); //TODO: figure out how to filter by role
            this.Students = group.Users.Select(x => x.FullName).ToList(); //TODO: figure out how to filter by role
        }
    }
}