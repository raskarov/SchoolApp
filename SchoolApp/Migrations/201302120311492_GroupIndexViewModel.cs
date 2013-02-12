namespace DefaultConnection.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GroupIndexViewModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Groups", "Discriminator");
        }
    }
}
