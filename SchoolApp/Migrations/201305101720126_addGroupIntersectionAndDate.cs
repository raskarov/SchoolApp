namespace SchoolApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addGroupIntersectionAndDate : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Groups", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Groups", "ParentGroup_GroupId", c => c.Int());
            AddForeignKey("dbo.Groups", "ParentGroup_GroupId", "dbo.Groups", "GroupId");
            CreateIndex("dbo.Groups", "ParentGroup_GroupId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Groups", new[] { "ParentGroup_GroupId" });
            DropForeignKey("dbo.Groups", "ParentGroup_GroupId", "dbo.Groups");
            DropColumn("dbo.Groups", "ParentGroup_GroupId");
            DropColumn("dbo.Groups", "CreatedDate");
        }
    }
}
