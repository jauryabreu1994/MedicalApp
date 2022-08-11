namespace MedicalApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NinthMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClienteDocumento", "Name", c => c.String());
            AddColumn("dbo.ClienteDocumento", "Extension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClienteDocumento", "Extension");
            DropColumn("dbo.ClienteDocumento", "Name");
        }
    }
}
