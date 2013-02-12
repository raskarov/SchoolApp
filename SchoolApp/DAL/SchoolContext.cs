using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DefaultConnection.Models;

namespace DefaultConnection.DAL
{
    public class SchoolContext :DbContext
    {

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
        }
        public DbSet<Classroom> Classrooms { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}