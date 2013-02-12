namespace DefaultConnection.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GroupIndexViewModel1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Groups", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Groups", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
    }
}
