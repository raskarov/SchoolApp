namespace SchoolApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPaymentProfileToGroup : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "PaymentProfileId", c => c.Int());
            AddForeignKey("dbo.Groups", "PaymentProfileId", "dbo.PaymentProfiles", "PaymentProfileId");
            CreateIndex("dbo.Groups", "PaymentProfileId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Groups", new[] { "PaymentProfileId" });
            DropForeignKey("dbo.Groups", "PaymentProfileId", "dbo.PaymentProfiles");
            DropColumn("dbo.Groups", "PaymentProfileId");
        }
    }
}
