namespace SchoolApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStudentCreationDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "CreationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfile", "CreationDate");
        }
    }
}
