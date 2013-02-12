using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DefaultConnection.Models;

namespace SchoolApp
{
    public class UsersContext : DbContext
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}