using System.Configuration;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Security;
using WebMatrix.WebData;
using SchoolApp.Helpers;

namespace SchoolApp.Migrations
{
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

        /// <summary>
        /// Create default roles and users
        /// </summary>
        private void SeedRolesAndUsers()
        {
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", true);

            if (!Roles.RoleExists(CoreHelper.ADMIN_ROLE))
            {
                Roles.CreateRole(CoreHelper.ADMIN_ROLE);
            }

            if (!Roles.RoleExists(CoreHelper.TEACHER_ROLE))
            {
                Roles.CreateRole(CoreHelper.TEACHER_ROLE);
            }

            if (!Roles.RoleExists(CoreHelper.STUDENT_ROLE))
            {
                Roles.CreateRole(CoreHelper.STUDENT_ROLE);
            }

            if (!Roles.RoleExists(CoreHelper.REGISTERED_USER_ROLE))
            {
                Roles.CreateRole(CoreHelper.REGISTERED_USER_ROLE);
            }

            if (!WebSecurity.UserExists("raskarov"))
            {
                WebSecurity.CreateUserAndAccount("raskarov", "password", new { FirstName = "Ruslan", LastName = "Askarov", Email = "askarru@gmail.com" });
            }

            if (!WebSecurity.UserExists("testteacher"))
            {
                WebSecurity.CreateUserAndAccount("testteacher", "password", new { FirstName = "Teacher", LastName = "Test", Email = "askarru@gmail.com" });
            }

            if (!Roles.GetRolesForUser("raskarov").Contains(CoreHelper.ADMIN_ROLE))
            {
                Roles.AddUserToRole("raskarov", CoreHelper.ADMIN_ROLE);
            }

            if (!Roles.GetRolesForUser("testteacher").Contains(CoreHelper.TEACHER_ROLE))
            {
                Roles.AddUserToRole("testteacher", CoreHelper.TEACHER_ROLE);
            }
        }
    }
}
