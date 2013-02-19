namespace SchoolApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requiredClassroomName : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Classrooms", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Classrooms", "Name", c => c.String());
        }
    }
}
