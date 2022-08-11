namespace MedicalApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FourteenthMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TransaccionLinea", "ServicioId", "dbo.Servicio");
            DropForeignKey("dbo.TransaccionLinea", "TransaccionId", "dbo.Transaccion");
            DropForeignKey("dbo.TransaccionPago", "PagoId", "dbo.Pago");
            DropForeignKey("dbo.TransaccionPendienteLinea", "ProductoId", "dbo.Producto");
            DropForeignKey("dbo.TransaccionPendienteLinea", "ServicioId", "dbo.Servicio");
            DropForeignKey("dbo.TransaccionPendienteLinea", "TransaccionId", "dbo.TransaccionPendiente");
            DropForeignKey("dbo.TransaccionPendientePago", "TransaccionId", "dbo.TransaccionPendiente");
            DropForeignKey("dbo.TransaccionPendiente", "_Cajero_Id", "dbo.Usuario");
            DropForeignKey("dbo.TransaccionPendiente", "ClienteId", "dbo.Cliente");
            DropForeignKey("dbo.TransaccionPendiente", "_Doctor_Id", "dbo.Usuario");
            DropForeignKey("dbo.TransaccionPendientePago", "PagoId", "dbo.Pago");
            DropForeignKey("dbo.TransaccionPago", "TransaccionId", "dbo.Transaccion");
            DropForeignKey("dbo.Transaccion", "_Cajero_Id", "dbo.Usuario");
            DropForeignKey("dbo.Transaccion", "_Doctor_Id", "dbo.Usuario");
            DropForeignKey("dbo.TransaccionLinea", "ProductoId", "dbo.Producto");
            DropForeignKey("dbo.TransaccionLinea", "ImpuestoId", "dbo.Impuesto");
            DropForeignKey("dbo.TransaccionPendienteLinea", "ImpuestoId", "dbo.Impuesto");
            DropForeignKey("dbo.Transaccion", "EmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.TransaccionPendiente", "EmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.Transaccion", "ClienteId", "dbo.Cliente");
            DropForeignKey("dbo.Transaccion", "EnfermeraId", "dbo.Usuario");
            DropForeignKey("dbo.TransaccionPendiente", "EnfermeraId", "dbo.Usuario");
            DropIndex("dbo.TransaccionLinea", new[] { "TransaccionId" });
            DropIndex("dbo.TransaccionLinea", new[] { "ServicioId" });
            DropIndex("dbo.TransaccionLinea", new[] { "ProductoId" });
            DropIndex("dbo.TransaccionLinea", new[] { "ImpuestoId" });
            DropIndex("dbo.Transaccion", new[] { "EmpresaId" });
            DropIndex("dbo.Transaccion", new[] { "ClienteId" });
            DropIndex("dbo.Transaccion", new[] { "EnfermeraId" });
            DropIndex("dbo.Transaccion", new[] { "_Cajero_Id" });
            DropIndex("dbo.Transaccion", new[] { "_Doctor_Id" });
            DropIndex("dbo.TransaccionPago", new[] { "TransaccionId" });
            DropIndex("dbo.TransaccionPago", new[] { "PagoId" });
            DropIndex("dbo.TransaccionPendientePago", new[] { "TransaccionId" });
            DropIndex("dbo.TransaccionPendientePago", new[] { "PagoId" });
            DropIndex("dbo.TransaccionPendiente", new[] { "EmpresaId" });
            DropIndex("dbo.TransaccionPendiente", new[] { "ClienteId" });
            DropIndex("dbo.TransaccionPendiente", new[] { "EnfermeraId" });
            DropIndex("dbo.TransaccionPendiente", new[] { "_Cajero_Id" });
            DropIndex("dbo.TransaccionPendiente", new[] { "_Doctor_Id" });
            DropIndex("dbo.TransaccionPendienteLinea", new[] { "TransaccionId" });
            DropIndex("dbo.TransaccionPendienteLinea", new[] { "ServicioId" });
            DropIndex("dbo.TransaccionPendienteLinea", new[] { "ProductoId" });
            DropIndex("dbo.TransaccionPendienteLinea", new[] { "ImpuestoId" });
            DropTable("dbo.TransaccionLinea");
            DropTable("dbo.Transaccion");
            DropTable("dbo.TransaccionPago");
            DropTable("dbo.TransaccionPendientePago");
            DropTable("dbo.TransaccionPendiente");
            DropTable("dbo.TransaccionPendienteLinea");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TransaccionPendienteLinea",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransaccionId = c.Int(nullable: false),
                        Linea = c.Int(nullable: false),
                        ServicioId = c.Int(nullable: false),
                        ProductoId = c.Int(nullable: false),
                        ImpuestoId = c.Int(nullable: false),
                        Cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Costo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Impuestos = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Descuento = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AplicaDevolucion = c.Boolean(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TransaccionPendiente",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmpresaId = c.Int(nullable: false),
                        TransaccionId = c.String(maxLength: 20),
                        ClienteId = c.Int(nullable: false),
                        CajeroId = c.Int(nullable: false),
                        DoctorId = c.Int(nullable: false),
                        EnfermeraId = c.Int(nullable: false),
                        SubTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Descuento = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Impuestos = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EsDevolucion = c.Boolean(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                        _Cajero_Id = c.Int(),
                        _Doctor_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TransaccionPendientePago",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransaccionId = c.Int(nullable: false),
                        Linea = c.Int(nullable: false),
                        PagoId = c.Int(nullable: false),
                        Monto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TasaCambio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TransaccionPago",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransaccionId = c.Int(nullable: false),
                        Linea = c.Int(nullable: false),
                        PagoId = c.Int(nullable: false),
                        Monto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TasaCambio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Transaccion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmpresaId = c.Int(nullable: false),
                        TransaccionId = c.String(maxLength: 20),
                        ClienteId = c.Int(nullable: false),
                        CajeroId = c.Int(nullable: false),
                        DoctorId = c.Int(nullable: false),
                        EnfermeraId = c.Int(nullable: false),
                        SubTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Impuestos = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Descuento = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EsDevolucion = c.Boolean(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                        _Cajero_Id = c.Int(),
                        _Doctor_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TransaccionLinea",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransaccionId = c.Int(nullable: false),
                        Linea = c.Int(nullable: false),
                        ServicioId = c.Int(nullable: false),
                        ProductoId = c.Int(nullable: false),
                        ImpuestoId = c.Int(nullable: false),
                        Cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Costo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Impuestos = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Descuento = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AplicaDevolucion = c.Boolean(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.TransaccionPendienteLinea", "ImpuestoId");
            CreateIndex("dbo.TransaccionPendienteLinea", "ProductoId");
            CreateIndex("dbo.TransaccionPendienteLinea", "ServicioId");
            CreateIndex("dbo.TransaccionPendienteLinea", "TransaccionId");
            CreateIndex("dbo.TransaccionPendiente", "_Doctor_Id");
            CreateIndex("dbo.TransaccionPendiente", "_Cajero_Id");
            CreateIndex("dbo.TransaccionPendiente", "EnfermeraId");
            CreateIndex("dbo.TransaccionPendiente", "ClienteId");
            CreateIndex("dbo.TransaccionPendiente", "EmpresaId");
            CreateIndex("dbo.TransaccionPendientePago", "PagoId");
            CreateIndex("dbo.TransaccionPendientePago", "TransaccionId");
            CreateIndex("dbo.TransaccionPago", "PagoId");
            CreateIndex("dbo.TransaccionPago", "TransaccionId");
            CreateIndex("dbo.Transaccion", "_Doctor_Id");
            CreateIndex("dbo.Transaccion", "_Cajero_Id");
            CreateIndex("dbo.Transaccion", "EnfermeraId");
            CreateIndex("dbo.Transaccion", "ClienteId");
            CreateIndex("dbo.Transaccion", "EmpresaId");
            CreateIndex("dbo.TransaccionLinea", "ImpuestoId");
            CreateIndex("dbo.TransaccionLinea", "ProductoId");
            CreateIndex("dbo.TransaccionLinea", "ServicioId");
            CreateIndex("dbo.TransaccionLinea", "TransaccionId");
            AddForeignKey("dbo.TransaccionPendiente", "EnfermeraId", "dbo.Usuario", "Id");
            AddForeignKey("dbo.Transaccion", "EnfermeraId", "dbo.Usuario", "Id");
            AddForeignKey("dbo.Transaccion", "ClienteId", "dbo.Cliente", "Id");
            AddForeignKey("dbo.TransaccionPendiente", "EmpresaId", "dbo.Empresa", "Id");
            AddForeignKey("dbo.Transaccion", "EmpresaId", "dbo.Empresa", "Id");
            AddForeignKey("dbo.TransaccionPendienteLinea", "ImpuestoId", "dbo.Impuesto", "Id");
            AddForeignKey("dbo.TransaccionLinea", "ImpuestoId", "dbo.Impuesto", "Id");
            AddForeignKey("dbo.TransaccionLinea", "ProductoId", "dbo.Producto", "Id");
            AddForeignKey("dbo.Transaccion", "_Doctor_Id", "dbo.Usuario", "Id");
            AddForeignKey("dbo.Transaccion", "_Cajero_Id", "dbo.Usuario", "Id");
            AddForeignKey("dbo.TransaccionPago", "TransaccionId", "dbo.Transaccion", "Id");
            AddForeignKey("dbo.TransaccionPendientePago", "PagoId", "dbo.Pago", "Id");
            AddForeignKey("dbo.TransaccionPendiente", "_Doctor_Id", "dbo.Usuario", "Id");
            AddForeignKey("dbo.TransaccionPendiente", "ClienteId", "dbo.Cliente", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TransaccionPendiente", "_Cajero_Id", "dbo.Usuario", "Id");
            AddForeignKey("dbo.TransaccionPendientePago", "TransaccionId", "dbo.TransaccionPendiente", "Id");
            AddForeignKey("dbo.TransaccionPendienteLinea", "TransaccionId", "dbo.TransaccionPendiente", "Id");
            AddForeignKey("dbo.TransaccionPendienteLinea", "ServicioId", "dbo.Servicio", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TransaccionPendienteLinea", "ProductoId", "dbo.Producto", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TransaccionPago", "PagoId", "dbo.Pago", "Id");
            AddForeignKey("dbo.TransaccionLinea", "TransaccionId", "dbo.Transaccion", "Id");
            AddForeignKey("dbo.TransaccionLinea", "ServicioId", "dbo.Servicio", "Id");
        }
    }
}
