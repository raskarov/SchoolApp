namespace SchoolApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPhotoToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "Photo", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfile", "Photo");
        }
    }
}
