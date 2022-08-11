namespace MedicalApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FifthMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cita", "TipoCita", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cita", "TipoCita");
        }
    }
}
