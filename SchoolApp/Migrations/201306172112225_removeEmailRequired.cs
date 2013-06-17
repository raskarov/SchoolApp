namespace SchoolApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeEmailRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserProfile", "Email", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserProfile", "Email", c => c.String(nullable: false));
        }
    }
}
