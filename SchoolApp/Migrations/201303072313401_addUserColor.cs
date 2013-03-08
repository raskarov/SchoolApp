namespace SchoolApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUserColor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "HexColor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfile", "HexColor");
        }
    }
}
