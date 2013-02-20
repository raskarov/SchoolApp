namespace SchoolApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserGroupInstanceAddForeignKeys : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserGroupInstances", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.UserGroupInstances", "GroupId", c => c.Int(nullable: false));
            AddForeignKey("dbo.UserGroupInstances", "UserId", "dbo.UserProfile", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.UserGroupInstances", "GroupId", "dbo.Groups", "GroupId", cascadeDelete: true);
            CreateIndex("dbo.UserGroupInstances", "UserId");
            CreateIndex("dbo.UserGroupInstances", "GroupId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserGroupInstances", new[] { "GroupId" });
            DropIndex("dbo.UserGroupInstances", new[] { "UserId" });
            DropForeignKey("dbo.UserGroupInstances", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.UserGroupInstances", "UserId", "dbo.UserProfile");
            DropColumn("dbo.UserGroupInstances", "GroupId");
            DropColumn("dbo.UserGroupInstances", "UserId");
        }
    }
}
