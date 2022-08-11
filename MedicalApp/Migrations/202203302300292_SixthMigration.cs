namespace MedicalApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SixthMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Producto", "ProductoId", c => c.String(maxLength: 20));
            AddColumn("dbo.Producto", "DescripcionExtendida", c => c.String(maxLength: 500));
            AddColumn("dbo.Servicio", "DescripcionExtendida", c => c.String(maxLength: 500));
            DropColumn("dbo.Producto", "ServicioId");
            DropColumn("dbo.Producto", "DescriptionExtendida");
            DropColumn("dbo.Servicio", "DescriptionExtendida");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Servicio", "DescriptionExtendida", c => c.String(maxLength: 500));
            AddColumn("dbo.Producto", "DescriptionExtendida", c => c.String(maxLength: 500));
            AddColumn("dbo.Producto", "ServicioId", c => c.String(maxLength: 20));
            DropColumn("dbo.Servicio", "DescripcionExtendida");
            DropColumn("dbo.Producto", "DescripcionExtendida");
            DropColumn("dbo.Producto", "ProductoId");
        }
    }
}
