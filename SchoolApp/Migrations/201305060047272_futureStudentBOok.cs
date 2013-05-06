namespace SchoolApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class futureStudentBOok : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "FutureStudent", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfile", "FutureStudent");
        }
    }
}
