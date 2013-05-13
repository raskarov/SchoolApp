using System.Configuration;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Security;
using WebMatrix.WebData;
using SchoolApp.Extensions;

namespace SchoolApp.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<SchoolApp.DAL.SchoolContext>
    {
        public Configuration()
        {
           
            AutomaticMigrationsEnabled = true;
        }
        protected override void Seed(SchoolApp.DAL.SchoolContext context)
        {
            SeedRolesAndUsers();
        }

        /// <summary>
        /// Create default roles and users
        /// </summary>
        private void SeedRolesAndUsers()
        {
            //WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", true);

            if (!Roles.RoleExists(Helpers.ADMIN_ROLE))
            {
                Roles.CreateRole(Helpers.ADMIN_ROLE);
            }

            if (!Roles.RoleExists(Helpers.TEACHER_ROLE))
            {
                Roles.CreateRole(Helpers.TEACHER_ROLE);
            }

            if (!Roles.RoleExists(Helpers.STUDENT_ROLE))
            {
                Roles.CreateRole(Helpers.STUDENT_ROLE);
            }

            if (!Roles.RoleExists(Helpers.REGISTERED_USER_ROLE))
            {
                Roles.CreateRole(Helpers.REGISTERED_USER_ROLE);
            }

            if (!WebSecurity.UserExists("raskarov"))
            {
                WebSecurity.CreateUserAndAccount("raskarov", "password", new { FirstName = "Ruslan", LastName = "Askarov", Email = "askarru@gmail.com" });
            }

            if (!WebSecurity.UserExists("testteacher"))
            {
                WebSecurity.CreateUserAndAccount("testteacher", "password", new { FirstName = "Teacher", LastName = "Test", Email = "askarru@gmail.com" });
            }

            if (!Roles.GetRolesForUser("raskarov").Contains(Helpers.ADMIN_ROLE))
            {
                Roles.AddUserToRole("raskarov", Helpers.ADMIN_ROLE);
            }

            if (!Roles.GetRolesForUser("testteacher").Contains(Helpers.TEACHER_ROLE))
            {
                Roles.AddUserToRole("testteacher", Helpers.TEACHER_ROLE);
            }
        }
    }
}
