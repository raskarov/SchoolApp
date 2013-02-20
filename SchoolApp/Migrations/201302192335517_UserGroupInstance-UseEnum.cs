namespace SchoolApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserGroupInstanceUseEnum : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserGroupInstances", "Present", c => c.Int(nullable: false));
            DropColumn("dbo.UserGroupInstances", "Absent");
            DropColumn("dbo.UserGroupInstances", "AbsentWithExcuse");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserGroupInstances", "AbsentWithExcuse", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserGroupInstances", "Absent", c => c.Boolean(nullable: false));
            AlterColumn("dbo.UserGroupInstances", "Present", c => c.Boolean(nullable: false));
        }
    }
}
