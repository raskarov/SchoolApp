namespace DefaultConnection.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDataAnnotationsToUserProfile : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserProfile", "UserName", c => c.String(nullable: false));
            AlterColumn("dbo.UserProfile", "FirstName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.UserProfile", "LastName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.UserProfile", "Email", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserProfile", "Email", c => c.String());
            AlterColumn("dbo.UserProfile", "LastName", c => c.String());
            AlterColumn("dbo.UserProfile", "FirstName", c => c.String());
            AlterColumn("dbo.UserProfile", "UserName", c => c.String());
        }
    }
}
