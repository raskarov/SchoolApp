namespace DefaultConnection.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPaymentProfile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PaymentProfiles",
                c => new
                    {
                        PaymentProfileId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Comments = c.String(),
                    })
                .PrimaryKey(t => t.PaymentProfileId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PaymentProfiles");
        }
    }
}
