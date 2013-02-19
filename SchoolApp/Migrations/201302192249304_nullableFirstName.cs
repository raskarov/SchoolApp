namespace SchoolApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullableFirstName : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserProfile", "FirstName", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserProfile", "FirstName", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
