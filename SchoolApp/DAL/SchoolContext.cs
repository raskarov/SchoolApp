using System.Data.Entity;
using System.Linq;
using System.Web.Security;
using SchoolApp.Extensions;
using SchoolApp.Models;
using WebMatrix.WebData;
using SchoolApp.Filters;
using System.Data.Objects;
using System.Collections.Generic;
using System.Data;
using Isg.EntityFramework;

namespace SchoolApp.DAL
{
    public class SchoolContext : DbContextBase
    {
        public DbSet<Classroom> Classrooms { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<PaymentProfile> PaymentProfiles { get; set; }

        public DbSet<PaymentRule> PaymentRules { get; set; }

        public DbSet<UserGroupInstance> UserGroupInstances { get; set; }

        public DbSet<GroupInstance> GroupInstances { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public IQueryable<UserProfile> Students
        {
            get
            {
                var AllStudents = Roles.GetUsersInRole(Helpers.STUDENT_ROLE);
                return UserProfiles.Where(x => AllStudents.Contains(x.UserName) && x.FutureStudent == false);
            }
        }
        public IQueryable<UserProfile> FutureStudents
        {
            get
            {
                var AllStudents = Roles.GetUsersInRole(Helpers.STUDENT_ROLE);
                return UserProfiles.Where(x => AllStudents.Contains(x.UserName) && x.FutureStudent == true);
            }
        }
        public IQueryable<UserProfile> Teachers
        {
            get
            {
                var AllTeachers = Roles.GetUsersInRole(Helpers.TEACHER_ROLE);
                return UserProfiles.Where(x => AllTeachers.Contains(x.UserName));
            }
        }

        public DbSet<Guardian> Guardians { get; set; }
    }
}