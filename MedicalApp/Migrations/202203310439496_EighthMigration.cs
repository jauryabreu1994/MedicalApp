namespace MedicalApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EighthMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Producto", "Costo", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Producto", "Venta", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Servicio", "Costo", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Servicio", "Costo", c => c.String());
            AlterColumn("dbo.Producto", "Venta", c => c.String());
            AlterColumn("dbo.Producto", "Costo", c => c.String());
        }
    }
}
