namespace SchoolApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStudentRemarks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "Remarks", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfile", "Remarks");
        }
    }
}
