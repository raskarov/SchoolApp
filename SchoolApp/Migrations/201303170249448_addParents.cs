namespace SchoolApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addParents : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Guardians",
                c => new
                    {
                        GuardianId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        Relationship = c.String(),
                        UserProfile_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.GuardianId)
                .ForeignKey("dbo.UserProfile", t => t.UserProfile_UserId)
                .Index(t => t.UserProfile_UserId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Guardians", new[] { "UserProfile_UserId" });
            DropForeignKey("dbo.Guardians", "UserProfile_UserId", "dbo.UserProfile");
            DropTable("dbo.Guardians");
        }
    }
}
