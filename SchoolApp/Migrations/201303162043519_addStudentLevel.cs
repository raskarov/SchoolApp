namespace SchoolApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStudentLevel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "StudentLevel", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfile", "StudentLevel");
        }
    }
}
