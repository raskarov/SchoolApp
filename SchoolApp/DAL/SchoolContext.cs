using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Security;
using SchoolApp.Models;

namespace SchoolApp.DAL
{
    public class SchoolContext :DbContext
    {
        public DbSet<Classroom> Classrooms { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public IQueryable<UserProfile> Students
        {
            get
            {
                var AllStudents = Roles.GetUsersInRole("Student");
                return UserProfiles.Where(x => AllStudents.Contains(x.UserName));
            }
        }
        public IQueryable<UserProfile> Teachers
        {
            get
            {
                var AllTeachers = Roles.GetUsersInRole("Teacher");
                return UserProfiles.Where(x=>AllTeachers.Contains(x.UserName));
            }
        }
        public DbSet<Group> Groups { get; set; }

        public DbSet<PaymentProfile> PaymentProfiles { get; set; }

        public DbSet<PaymentRule> PaymentRules { get; set; }

        public DbSet<GroupInstance> GroupInstances { get; set; }
    }
}