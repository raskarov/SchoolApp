namespace SchoolApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsDeletedToModels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Classrooms", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserProfile", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Groups", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.PaymentProfiles", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Guardians", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.PaymentRules", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserGroupInstances", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.GroupInstances", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Payments", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payments", "IsDeleted");
            DropColumn("dbo.GroupInstances", "IsDeleted");
            DropColumn("dbo.UserGroupInstances", "IsDeleted");
            DropColumn("dbo.PaymentRules", "IsDeleted");
            DropColumn("dbo.Guardians", "IsDeleted");
            DropColumn("dbo.PaymentProfiles", "IsDeleted");
            DropColumn("dbo.Groups", "IsDeleted");
            DropColumn("dbo.UserProfile", "IsDeleted");
            DropColumn("dbo.Classrooms", "IsDeleted");
        }
    }
}
