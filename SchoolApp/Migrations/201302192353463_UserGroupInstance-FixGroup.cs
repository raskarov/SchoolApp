namespace SchoolApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserGroupInstanceFixGroup : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserGroupInstances", "GroupId", "dbo.Groups");
            DropIndex("dbo.UserGroupInstances", new[] { "GroupId" });
            AddColumn("dbo.UserGroupInstances", "GroupInstanceId", c => c.Int(nullable: false));
            AddForeignKey("dbo.UserGroupInstances", "GroupInstanceId", "dbo.GroupInstances", "GroupInstanceId", cascadeDelete: true);
            CreateIndex("dbo.UserGroupInstances", "GroupInstanceId");
            DropColumn("dbo.UserGroupInstances", "GroupId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserGroupInstances", "GroupId", c => c.Int(nullable: false));
            DropIndex("dbo.UserGroupInstances", new[] { "GroupInstanceId" });
            DropForeignKey("dbo.UserGroupInstances", "GroupInstanceId", "dbo.GroupInstances");
            DropColumn("dbo.UserGroupInstances", "GroupInstanceId");
            CreateIndex("dbo.UserGroupInstances", "GroupId");
            AddForeignKey("dbo.UserGroupInstances", "GroupId", "dbo.Groups", "GroupId", cascadeDelete: true);
        }
    }
}
