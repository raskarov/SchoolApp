namespace SchoolApp.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;
    using WebMatrix.WebData;

    internal sealed class Configuration : DbMigrationsConfiguration<SchoolApp.DAL.SchoolContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SchoolApp.DAL.SchoolContext context)
        {
            SeedRolesAndUsers();

        }
        private void SeedRolesAndUsers()
        {
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", true);

            if (!Roles.RoleExists("Administrator"))
            {
                Roles.CreateRole("Administrator");
            }
            if (!Roles.RoleExists("Teacher"))
            {
                Roles.CreateRole("Teacher");
            }
            if (!Roles.RoleExists("Student"))
            {
                Roles.CreateRole("Student");
            }
            if (!WebSecurity.UserExists("raskarov"))
            {
                WebSecurity.CreateUserAndAccount("raskarov", "password", new { FirstName = "Ruslan", LastName = "Askarov", Email = "askarru@gmail.com" });
            }
            if (!WebSecurity.UserExists("testteacher"))
            {
                WebSecurity.CreateUserAndAccount("testteacher", "password", new { FirstName = "Teacher", LastName = "Test", Email = "askarru@gmail.com" });
            }
            if (!Roles.GetRolesForUser("raskarov").Contains("Administrator"))
            {
                Roles.AddUserToRole("raskarov", "Administrator");
            }
            if (!Roles.GetRolesForUser("testteacher").Contains("Teacher"))
            {
                Roles.AddUserToRole("testteacher", "Teacher");
            }
        }
    }
}
