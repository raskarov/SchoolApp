namespace SchoolApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUserGroupInstance : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserGroupInstances",
                c => new
                    {
                        UserGroupInstanceID = c.Int(nullable: false, identity: true),
                        Present = c.Boolean(nullable: false),
                        Absent = c.Boolean(nullable: false),
                        AbsentWithExcuse = c.Boolean(nullable: false),
                        AttendanceTaken = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserGroupInstanceID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserGroupInstances");
        }
    }
}
