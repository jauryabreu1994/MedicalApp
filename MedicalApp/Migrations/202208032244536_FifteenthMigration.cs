namespace MedicalApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FifteenthMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductoClientes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HabitacionClienteId = c.Int(nullable: false),
                        ProductoId = c.Int(nullable: false),
                        Cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Descripcion = c.String(maxLength: 100),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Producto", t => t.ProductoId)
                .ForeignKey("dbo.HabitacionCliente", t => t.HabitacionClienteId)
                .Index(t => t.HabitacionClienteId)
                .Index(t => t.ProductoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductoClientes", "HabitacionClienteId", "dbo.HabitacionCliente");
            DropForeignKey("dbo.ProductoClientes", "ProductoId", "dbo.Producto");
            DropIndex("dbo.ProductoClientes", new[] { "ProductoId" });
            DropIndex("dbo.ProductoClientes", new[] { "HabitacionClienteId" });
            DropTable("dbo.ProductoClientes");
        }
    }
}
