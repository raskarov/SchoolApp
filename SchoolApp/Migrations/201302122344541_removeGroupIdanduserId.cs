namespace DefaultConnection.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeGroupIdanduserId : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UserProfile", "GroupId");
            DropColumn("dbo.Groups", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Groups", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.UserProfile", "GroupId", c => c.Int(nullable: false));
        }
    }
}
