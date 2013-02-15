namespace SchoolApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NonRequiredUsername : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserProfile", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserProfile", "UserName", c => c.String(nullable: false));
        }
    }
}
