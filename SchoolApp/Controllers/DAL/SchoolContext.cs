using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SchoolApp.Models;

namespace SchoolApp.DAL
{
    public class SchoolContext :DbContext
    {
        public DbSet<Classroom> Classrooms { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<PaymentProfile> PaymentProfiles { get; set; }

        public DbSet<PaymentRule> PaymentRules { get; set; }

        public DbSet<GroupInstance> GroupInstances { get; set; }
    }
}