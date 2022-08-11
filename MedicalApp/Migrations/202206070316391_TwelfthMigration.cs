namespace MedicalApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TwelfthMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cliente", "FechaNacimiento", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cliente", "FechaNacimiento");
        }
    }
}
