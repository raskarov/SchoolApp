namespace DefaultConnection.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addGroup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        GroupId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GroupId);
            
            CreateTable(
                "dbo.GroupUserProfiles",
                c => new
                    {
                        Group_GroupId = c.Int(nullable: false),
                        UserProfile_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Group_GroupId, t.UserProfile_UserId })
                .ForeignKey("dbo.Groups", t => t.Group_GroupId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserProfile_UserId, cascadeDelete: true)
                .Index(t => t.Group_GroupId)
                .Index(t => t.UserProfile_UserId);
            
            AddColumn("dbo.UserProfile", "GroupId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropIndex("dbo.GroupUserProfiles", new[] { "UserProfile_UserId" });
            DropIndex("dbo.GroupUserProfiles", new[] { "Group_GroupId" });
            DropForeignKey("dbo.GroupUserProfiles", "UserProfile_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.GroupUserProfiles", "Group_GroupId", "dbo.Groups");
            DropColumn("dbo.UserProfile", "GroupId");
            DropTable("dbo.GroupUserProfiles");
            DropTable("dbo.Groups");
        }
    }
}
