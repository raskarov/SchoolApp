namespace DefaultConnection.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentProfileRules : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentRules", "CreatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentRules", "CreatedDate");
        }
    }
}
