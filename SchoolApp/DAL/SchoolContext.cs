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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
        }

        public DbSet<Classroom> Classrooms { get; set; }

      
    }
}