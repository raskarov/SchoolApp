namespace SchoolApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addREcurrenceRule : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GroupInstances", "RecurrenceRule", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GroupInstances", "RecurrenceRule");
        }
    }
}
