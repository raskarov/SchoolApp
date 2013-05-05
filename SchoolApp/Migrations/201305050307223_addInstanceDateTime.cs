namespace SchoolApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addInstanceDateTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserGroupInstances", "InstanceDateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserGroupInstances", "InstanceDateTime");
        }
    }
}
