namespace SchoolApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeAuditGuardian : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Guardians", "CreateDate");
            DropColumn("dbo.Guardians", "CreateUser");
            DropColumn("dbo.Guardians", "UpdateDate");
            DropColumn("dbo.Guardians", "UpdateUser");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Guardians", "UpdateUser", c => c.String());
            AddColumn("dbo.Guardians", "UpdateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Guardians", "CreateUser", c => c.String());
            AddColumn("dbo.Guardians", "CreateDate", c => c.DateTime(nullable: false));
        }
    }
}
