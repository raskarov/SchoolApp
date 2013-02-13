namespace DefaultConnection.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPaymentRule : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PaymentRules",
                c => new
                    {
                        PaymentRuleId = c.Int(nullable: false, identity: true),
                        Rule = c.String(),
                        EffectiveDate = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Variable = c.Boolean(nullable: false),
                        PaymentProfileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PaymentRuleId)
                .ForeignKey("dbo.PaymentProfiles", t => t.PaymentProfileId, cascadeDelete: true)
                .Index(t => t.PaymentProfileId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.PaymentRules", new[] { "PaymentProfileId" });
            DropForeignKey("dbo.PaymentRules", "PaymentProfileId", "dbo.PaymentProfiles");
            DropTable("dbo.PaymentRules");
        }
    }
}
