namespace DefaultConnection.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;
    using WebMatrix.WebData;

    internal sealed class Configuration : DbMigrationsConfiguration<DefaultConnection.DAL.SchoolContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DefaultConnection.DAL.SchoolContext context)
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
            if (!WebSecurity.UserExists("raskarov"))
            {
                WebSecurity.CreateUserAndAccount("raskarov", "password", new { FirstName = "Ruslan", LastName = "Askarov" });
            }
            if (!WebSecurity.UserExists("testteacher"))
            {
                WebSecurity.CreateUserAndAccount("testteacher", "password", new { FirstName = "Teacher", LastName = "Test", Email="askarru@gmail.com" });
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
