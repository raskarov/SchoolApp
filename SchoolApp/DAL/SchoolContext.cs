﻿using System.Data.Entity;
using System.Linq;
using System.Web.Security;
using SchoolApp.Helpers;
using SchoolApp.Models;

namespace SchoolApp.DAL
{
    public class SchoolContext : DbContext
    {
        public DbSet<Classroom> Classrooms { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<PaymentProfile> PaymentProfiles { get; set; }

        public DbSet<PaymentRule> PaymentRules { get; set; }

        public DbSet<UserGroupInstance> UserGroupInstances { get; set; }

        public DbSet<GroupInstance> GroupInstances { get; set; }

        public IQueryable<UserProfile> Students
        {
            get
            {
                var AllStudents = Roles.GetUsersInRole(CoreHelper.STUDENT_ROLE);
                return UserProfiles.Where(x => AllStudents.Contains(x.UserName));
            }
        }
        public IQueryable<UserProfile> Teachers
        {
            get
            {
                var AllTeachers = Roles.GetUsersInRole(CoreHelper.TEACHER_ROLE);
                return UserProfiles.Where(x => AllTeachers.Contains(x.UserName));
            }
        }
      
    }
}