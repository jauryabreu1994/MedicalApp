namespace MedicalApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AreaEspecialidad",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AreaGeneralId = c.Int(nullable: false),
                        Descripcion = c.String(maxLength: 100),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AreaGeneral", t => t.AreaGeneralId)
                .Index(t => t.AreaGeneralId);
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmpresaId = c.Int(nullable: false),
                        GrupoUsuarioId = c.Int(nullable: false),
                        AreaEspecialidadId = c.Int(nullable: false),
                        UsuarioId = c.String(maxLength: 20),
                        Nombre = c.String(maxLength: 100),
                        Apellido = c.String(maxLength: 100),
                        Identificacion = c.String(maxLength: 20),
                        PaisId = c.Int(nullable: false),
                        CiudadId = c.Int(nullable: false),
                        Direccion = c.String(maxLength: 300),
                        Correo = c.String(maxLength: 100),
                        Telefono = c.String(maxLength: 30),
                        Genero = c.Int(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ciudad", t => t.CiudadId)
                .ForeignKey("dbo.Pais", t => t.PaisId)
                .ForeignKey("dbo.GrupoUsuario", t => t.GrupoUsuarioId)
                .ForeignKey("dbo.Empresa", t => t.EmpresaId)
                .ForeignKey("dbo.AreaEspecialidad", t => t.AreaEspecialidadId)
                .Index(t => t.EmpresaId)
                .Index(t => t.GrupoUsuarioId)
                .Index(t => t.AreaEspecialidadId)
                .Index(t => t.PaisId)
                .Index(t => t.CiudadId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cliente", t => t.ClienteId)
                .ForeignKey("dbo.Empresa", t => t.EmpresaId)
                .ForeignKey("dbo.Usuario", t => t._Cajero_Id)
                .ForeignKey("dbo.Usuario", t => t._Doctor_Id)
                .ForeignKey("dbo.Usuario", t => t.EnfermeraId)
                .Index(t => t.EmpresaId)
                .Index(t => t.ClienteId)
                .Index(t => t.EnfermeraId)
                .Index(t => t._Cajero_Id)
                .Index(t => t._Doctor_Id);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Producto", t => t.ProductoId)
                .ForeignKey("dbo.Servicio", t => t.ServicioId)
                .ForeignKey("dbo.Impuesto", t => t.ImpuestoId)
                .ForeignKey("dbo.Transaccion", t => t.TransaccionId)
                .Index(t => t.TransaccionId)
                .Index(t => t.ServicioId)
                .Index(t => t.ProductoId)
                .Index(t => t.ImpuestoId);
            
            CreateTable(
                "dbo.Impuesto",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmpresaId = c.Int(nullable: false),
                        Descripcion = c.String(maxLength: 100),
                        Monto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Prefijo = c.String(maxLength: 10),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Empresa", t => t.EmpresaId)
                .Index(t => t.EmpresaId);
            
            CreateTable(
                "dbo.Producto",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmpresaId = c.Int(nullable: false),
                        ServicioId = c.String(maxLength: 20),
                        Descripcion = c.String(maxLength: 100),
                        DescriptionExtendida = c.String(maxLength: 500),
                        Costo = c.String(),
                        Venta = c.String(),
                        ImpuestoId = c.Int(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                        _Usuario_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Empresa", t => t.EmpresaId)
                .ForeignKey("dbo.Usuario", t => t._Usuario_Id)
                .ForeignKey("dbo.Impuesto", t => t.ImpuestoId)
                .Index(t => t.EmpresaId)
                .Index(t => t.ImpuestoId)
                .Index(t => t._Usuario_Id);
            
            CreateTable(
                "dbo.Empresa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(maxLength: 100),
                        Identificacion = c.String(maxLength: 20),
                        NombreFiscal = c.String(maxLength: 100),
                        PaisId = c.Int(nullable: false),
                        CiudadId = c.Int(nullable: false),
                        Direccion = c.String(maxLength: 300),
                        Correo = c.String(maxLength: 100),
                        Telefono = c.String(maxLength: 30),
                        Logo = c.String(),
                        Moneda = c.Int(nullable: false),
                        CodiLicencia = c.String(),
                        ServicioId = c.Int(nullable: false),
                        ClienteId = c.Int(nullable: false),
                        CitaId = c.Int(nullable: false),
                        TransaccionId = c.Int(nullable: false),
                        CierreId = c.Int(nullable: false),
                        DoctorId = c.Int(nullable: false),
                        EnfermeraId = c.Int(nullable: false),
                        AdministrativoId = c.Int(nullable: false),
                        CorreoSMTP = c.String(maxLength: 100),
                        ContrasenaSMTP = c.String(maxLength: 1000),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ciudad", t => t.CiudadId)
                .ForeignKey("dbo.Pais", t => t.PaisId)
                .Index(t => t.PaisId)
                .Index(t => t.CiudadId);
            
            CreateTable(
                "dbo.Cita",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CitaId = c.String(maxLength: 20),
                        EmpresaId = c.Int(nullable: false),
                        ClienteId = c.Int(nullable: false),
                        AreaEspecialidadId = c.Int(nullable: false),
                        UsuarioId = c.Int(nullable: false),
                        Comentario = c.String(),
                        FechaCita = c.DateTime(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AreaEspecialidad", t => t.AreaEspecialidadId, cascadeDelete: true)
                .ForeignKey("dbo.Cliente", t => t.ClienteId)
                .ForeignKey("dbo.Usuario", t => t.UsuarioId, cascadeDelete: true)
                .ForeignKey("dbo.Empresa", t => t.EmpresaId)
                .Index(t => t.EmpresaId)
                .Index(t => t.ClienteId)
                .Index(t => t.AreaEspecialidadId)
                .Index(t => t.UsuarioId);
            
            CreateTable(
                "dbo.Cliente",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClienteId = c.String(maxLength: 20),
                        Nombre = c.String(maxLength: 100),
                        Apellido = c.String(maxLength: 100),
                        Identificacion = c.String(maxLength: 20),
                        NombreFiscal = c.String(maxLength: 100),
                        PaisId = c.Int(nullable: false),
                        CiudadId = c.Int(nullable: false),
                        Direccion = c.String(maxLength: 300),
                        Correo = c.String(maxLength: 100),
                        Telefono = c.String(maxLength: 30),
                        Genero = c.Int(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ciudad", t => t.CiudadId)
                .ForeignKey("dbo.Pais", t => t.PaisId)
                .Index(t => t.PaisId)
                .Index(t => t.CiudadId);
            
            CreateTable(
                "dbo.ClienteContrasena",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClienteId = c.Int(nullable: false),
                        Contraseña = c.String(),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cliente", t => t.ClienteId)
                .Index(t => t.ClienteId);
            
            CreateTable(
                "dbo.ClienteHistorial",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClienteId = c.Int(nullable: false),
                        UsuarioId = c.Int(nullable: false),
                        Documentacion = c.String(),
                        Indicaciones = c.String(),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuario", t => t.UsuarioId, cascadeDelete: true)
                .ForeignKey("dbo.Cliente", t => t.ClienteId)
                .Index(t => t.ClienteId)
                .Index(t => t.UsuarioId);
            
            CreateTable(
                "dbo.ClienteDocumento",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClienteHistorialId = c.Int(nullable: false),
                        Codigo = c.Guid(nullable: false),
                        RutaDocumento = c.String(),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClienteHistorial", t => t.ClienteHistorialId)
                .Index(t => t.ClienteHistorialId);
            
            CreateTable(
                "dbo.HabitacionCliente",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HabitacionId = c.Int(nullable: false),
                        ClienteId = c.Int(nullable: false),
                        CajeroId = c.Int(nullable: false),
                        DoctorId = c.Int(nullable: false),
                        EnfermeraId = c.Int(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                        _Cajero_Id = c.Int(),
                        _Doctor_Id = c.Int(),
                        _Enfermera_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuario", t => t._Cajero_Id)
                .ForeignKey("dbo.Usuario", t => t._Doctor_Id)
                .ForeignKey("dbo.Usuario", t => t._Enfermera_Id)
                .ForeignKey("dbo.Habitacion", t => t.HabitacionId)
                .ForeignKey("dbo.Cliente", t => t.ClienteId)
                .Index(t => t.HabitacionId)
                .Index(t => t.ClienteId)
                .Index(t => t._Cajero_Id)
                .Index(t => t._Doctor_Id)
                .Index(t => t._Enfermera_Id);
            
            CreateTable(
                "dbo.Habitacion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EdificioNivelId = c.Int(nullable: false),
                        CodigoHabitacion = c.String(),
                        MaximoClientes = c.Int(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EdificioNivel", t => t.EdificioNivelId)
                .Index(t => t.EdificioNivelId);
            
            CreateTable(
                "dbo.EdificioNivel",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EdificioId = c.Int(nullable: false),
                        Nivel = c.Int(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Edificio", t => t.EdificioId)
                .Index(t => t.EdificioId);
            
            CreateTable(
                "dbo.Edificio",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmpresaId = c.Int(nullable: false),
                        Descripcion = c.String(),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Empresa", t => t.EmpresaId)
                .Index(t => t.EmpresaId);
            
            CreateTable(
                "dbo.Ciudad",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PaisId = c.Int(nullable: false),
                        Descripcion = c.String(maxLength: 100),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pais", t => t.PaisId)
                .Index(t => t.PaisId);
            
            CreateTable(
                "dbo.Pais",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CodigoArea = c.String(maxLength: 10),
                        Descripcion = c.String(maxLength: 100),
                        CodigoTelefono = c.String(maxLength: 10),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GrupoUsuario",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmpresaId = c.Int(nullable: false),
                        Descripcion = c.String(maxLength: 100),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Empresa", t => t.EmpresaId)
                .Index(t => t.EmpresaId);
            
            CreateTable(
                "dbo.GrupoPermiso",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GrupoUsuarioId = c.Int(nullable: false),
                        PermisoId = c.Int(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Permiso", t => t.PermisoId)
                .ForeignKey("dbo.GrupoUsuario", t => t.GrupoUsuarioId)
                .Index(t => t.GrupoUsuarioId)
                .Index(t => t.PermisoId);
            
            CreateTable(
                "dbo.Permiso",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(maxLength: 100),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pago",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmpresaId = c.Int(nullable: false),
                        Descripcion = c.String(),
                        TasaCambio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TipoPago = c.Int(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Empresa", t => t.EmpresaId)
                .Index(t => t.EmpresaId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pago", t => t.PagoId)
                .ForeignKey("dbo.Transaccion", t => t.TransaccionId)
                .Index(t => t.TransaccionId)
                .Index(t => t.PagoId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TransaccionPendiente", t => t.TransaccionId)
                .ForeignKey("dbo.Pago", t => t.PagoId)
                .Index(t => t.TransaccionId)
                .Index(t => t.PagoId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuario", t => t._Cajero_Id)
                .ForeignKey("dbo.Cliente", t => t.ClienteId, cascadeDelete: true)
                .ForeignKey("dbo.Usuario", t => t._Doctor_Id)
                .ForeignKey("dbo.Empresa", t => t.EmpresaId)
                .ForeignKey("dbo.Usuario", t => t.EnfermeraId)
                .Index(t => t.EmpresaId)
                .Index(t => t.ClienteId)
                .Index(t => t.EnfermeraId)
                .Index(t => t._Cajero_Id)
                .Index(t => t._Doctor_Id);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Producto", t => t.ProductoId, cascadeDelete: true)
                .ForeignKey("dbo.Servicio", t => t.ServicioId, cascadeDelete: true)
                .ForeignKey("dbo.TransaccionPendiente", t => t.TransaccionId)
                .ForeignKey("dbo.Impuesto", t => t.ImpuestoId)
                .Index(t => t.TransaccionId)
                .Index(t => t.ServicioId)
                .Index(t => t.ProductoId)
                .Index(t => t.ImpuestoId);
            
            CreateTable(
                "dbo.Servicio",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmpresaId = c.Int(nullable: false),
                        ServicioId = c.String(maxLength: 20),
                        Descripcion = c.String(maxLength: 100),
                        DescriptionExtendida = c.String(maxLength: 500),
                        UsuarioId = c.Int(nullable: false),
                        Costo = c.String(),
                        ImpuestoId = c.Int(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuario", t => t.UsuarioId, cascadeDelete: true)
                .ForeignKey("dbo.Empresa", t => t.EmpresaId)
                .ForeignKey("dbo.Impuesto", t => t.ImpuestoId)
                .Index(t => t.EmpresaId)
                .Index(t => t.UsuarioId)
                .Index(t => t.ImpuestoId);
            
            CreateTable(
                "dbo.UsuarioContrasena",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UsuarioId = c.Int(nullable: false),
                        Contraseña = c.String(),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuario", t => t.UsuarioId)
                .Index(t => t.UsuarioId);
            
            CreateTable(
                "dbo.AreaGeneral",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(maxLength: 100),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AreaEspecialidad", "AreaGeneralId", "dbo.AreaGeneral");
            DropForeignKey("dbo.Usuario", "AreaEspecialidadId", "dbo.AreaEspecialidad");
            DropForeignKey("dbo.UsuarioContrasena", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.TransaccionPendiente", "EnfermeraId", "dbo.Usuario");
            DropForeignKey("dbo.Transaccion", "EnfermeraId", "dbo.Usuario");
            DropForeignKey("dbo.Transaccion", "_Doctor_Id", "dbo.Usuario");
            DropForeignKey("dbo.Transaccion", "_Cajero_Id", "dbo.Usuario");
            DropForeignKey("dbo.TransaccionPago", "TransaccionId", "dbo.Transaccion");
            DropForeignKey("dbo.TransaccionLinea", "TransaccionId", "dbo.Transaccion");
            DropForeignKey("dbo.TransaccionPendienteLinea", "ImpuestoId", "dbo.Impuesto");
            DropForeignKey("dbo.TransaccionLinea", "ImpuestoId", "dbo.Impuesto");
            DropForeignKey("dbo.Servicio", "ImpuestoId", "dbo.Impuesto");
            DropForeignKey("dbo.Producto", "ImpuestoId", "dbo.Impuesto");
            DropForeignKey("dbo.Producto", "_Usuario_Id", "dbo.Usuario");
            DropForeignKey("dbo.Usuario", "EmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.TransaccionPendiente", "EmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.Transaccion", "EmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.Servicio", "EmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.Producto", "EmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.Pago", "EmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.TransaccionPendientePago", "PagoId", "dbo.Pago");
            DropForeignKey("dbo.TransaccionPendiente", "_Doctor_Id", "dbo.Usuario");
            DropForeignKey("dbo.TransaccionPendiente", "ClienteId", "dbo.Cliente");
            DropForeignKey("dbo.TransaccionPendiente", "_Cajero_Id", "dbo.Usuario");
            DropForeignKey("dbo.TransaccionPendientePago", "TransaccionId", "dbo.TransaccionPendiente");
            DropForeignKey("dbo.TransaccionPendienteLinea", "TransaccionId", "dbo.TransaccionPendiente");
            DropForeignKey("dbo.TransaccionPendienteLinea", "ServicioId", "dbo.Servicio");
            DropForeignKey("dbo.Servicio", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.TransaccionLinea", "ServicioId", "dbo.Servicio");
            DropForeignKey("dbo.TransaccionPendienteLinea", "ProductoId", "dbo.Producto");
            DropForeignKey("dbo.TransaccionPago", "PagoId", "dbo.Pago");
            DropForeignKey("dbo.Impuesto", "EmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.GrupoUsuario", "EmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.Usuario", "GrupoUsuarioId", "dbo.GrupoUsuario");
            DropForeignKey("dbo.GrupoPermiso", "GrupoUsuarioId", "dbo.GrupoUsuario");
            DropForeignKey("dbo.GrupoPermiso", "PermisoId", "dbo.Permiso");
            DropForeignKey("dbo.Edificio", "EmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.Cita", "EmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.Cita", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.Usuario", "PaisId", "dbo.Pais");
            DropForeignKey("dbo.Empresa", "PaisId", "dbo.Pais");
            DropForeignKey("dbo.Cliente", "PaisId", "dbo.Pais");
            DropForeignKey("dbo.Ciudad", "PaisId", "dbo.Pais");
            DropForeignKey("dbo.Usuario", "CiudadId", "dbo.Ciudad");
            DropForeignKey("dbo.Empresa", "CiudadId", "dbo.Ciudad");
            DropForeignKey("dbo.Cliente", "CiudadId", "dbo.Ciudad");
            DropForeignKey("dbo.Transaccion", "ClienteId", "dbo.Cliente");
            DropForeignKey("dbo.HabitacionCliente", "ClienteId", "dbo.Cliente");
            DropForeignKey("dbo.EdificioNivel", "EdificioId", "dbo.Edificio");
            DropForeignKey("dbo.Habitacion", "EdificioNivelId", "dbo.EdificioNivel");
            DropForeignKey("dbo.HabitacionCliente", "HabitacionId", "dbo.Habitacion");
            DropForeignKey("dbo.HabitacionCliente", "_Enfermera_Id", "dbo.Usuario");
            DropForeignKey("dbo.HabitacionCliente", "_Doctor_Id", "dbo.Usuario");
            DropForeignKey("dbo.HabitacionCliente", "_Cajero_Id", "dbo.Usuario");
            DropForeignKey("dbo.ClienteHistorial", "ClienteId", "dbo.Cliente");
            DropForeignKey("dbo.ClienteHistorial", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.ClienteDocumento", "ClienteHistorialId", "dbo.ClienteHistorial");
            DropForeignKey("dbo.ClienteContrasena", "ClienteId", "dbo.Cliente");
            DropForeignKey("dbo.Cita", "ClienteId", "dbo.Cliente");
            DropForeignKey("dbo.Cita", "AreaEspecialidadId", "dbo.AreaEspecialidad");
            DropForeignKey("dbo.TransaccionLinea", "ProductoId", "dbo.Producto");
            DropIndex("dbo.UsuarioContrasena", new[] { "UsuarioId" });
            DropIndex("dbo.Servicio", new[] { "ImpuestoId" });
            DropIndex("dbo.Servicio", new[] { "UsuarioId" });
            DropIndex("dbo.Servicio", new[] { "EmpresaId" });
            DropIndex("dbo.TransaccionPendienteLinea", new[] { "ImpuestoId" });
            DropIndex("dbo.TransaccionPendienteLinea", new[] { "ProductoId" });
            DropIndex("dbo.TransaccionPendienteLinea", new[] { "ServicioId" });
            DropIndex("dbo.TransaccionPendienteLinea", new[] { "TransaccionId" });
            DropIndex("dbo.TransaccionPendiente", new[] { "_Doctor_Id" });
            DropIndex("dbo.TransaccionPendiente", new[] { "_Cajero_Id" });
            DropIndex("dbo.TransaccionPendiente", new[] { "EnfermeraId" });
            DropIndex("dbo.TransaccionPendiente", new[] { "ClienteId" });
            DropIndex("dbo.TransaccionPendiente", new[] { "EmpresaId" });
            DropIndex("dbo.TransaccionPendientePago", new[] { "PagoId" });
            DropIndex("dbo.TransaccionPendientePago", new[] { "TransaccionId" });
            DropIndex("dbo.TransaccionPago", new[] { "PagoId" });
            DropIndex("dbo.TransaccionPago", new[] { "TransaccionId" });
            DropIndex("dbo.Pago", new[] { "EmpresaId" });
            DropIndex("dbo.GrupoPermiso", new[] { "PermisoId" });
            DropIndex("dbo.GrupoPermiso", new[] { "GrupoUsuarioId" });
            DropIndex("dbo.GrupoUsuario", new[] { "EmpresaId" });
            DropIndex("dbo.Ciudad", new[] { "PaisId" });
            DropIndex("dbo.Edificio", new[] { "EmpresaId" });
            DropIndex("dbo.EdificioNivel", new[] { "EdificioId" });
            DropIndex("dbo.Habitacion", new[] { "EdificioNivelId" });
            DropIndex("dbo.HabitacionCliente", new[] { "_Enfermera_Id" });
            DropIndex("dbo.HabitacionCliente", new[] { "_Doctor_Id" });
            DropIndex("dbo.HabitacionCliente", new[] { "_Cajero_Id" });
            DropIndex("dbo.HabitacionCliente", new[] { "ClienteId" });
            DropIndex("dbo.HabitacionCliente", new[] { "HabitacionId" });
            DropIndex("dbo.ClienteDocumento", new[] { "ClienteHistorialId" });
            DropIndex("dbo.ClienteHistorial", new[] { "UsuarioId" });
            DropIndex("dbo.ClienteHistorial", new[] { "ClienteId" });
            DropIndex("dbo.ClienteContrasena", new[] { "ClienteId" });
            DropIndex("dbo.Cliente", new[] { "CiudadId" });
            DropIndex("dbo.Cliente", new[] { "PaisId" });
            DropIndex("dbo.Cita", new[] { "UsuarioId" });
            DropIndex("dbo.Cita", new[] { "AreaEspecialidadId" });
            DropIndex("dbo.Cita", new[] { "ClienteId" });
            DropIndex("dbo.Cita", new[] { "EmpresaId" });
            DropIndex("dbo.Empresa", new[] { "CiudadId" });
            DropIndex("dbo.Empresa", new[] { "PaisId" });
            DropIndex("dbo.Producto", new[] { "_Usuario_Id" });
            DropIndex("dbo.Producto", new[] { "ImpuestoId" });
            DropIndex("dbo.Producto", new[] { "EmpresaId" });
            DropIndex("dbo.Impuesto", new[] { "EmpresaId" });
            DropIndex("dbo.TransaccionLinea", new[] { "ImpuestoId" });
            DropIndex("dbo.TransaccionLinea", new[] { "ProductoId" });
            DropIndex("dbo.TransaccionLinea", new[] { "ServicioId" });
            DropIndex("dbo.TransaccionLinea", new[] { "TransaccionId" });
            DropIndex("dbo.Transaccion", new[] { "_Doctor_Id" });
            DropIndex("dbo.Transaccion", new[] { "_Cajero_Id" });
            DropIndex("dbo.Transaccion", new[] { "EnfermeraId" });
            DropIndex("dbo.Transaccion", new[] { "ClienteId" });
            DropIndex("dbo.Transaccion", new[] { "EmpresaId" });
            DropIndex("dbo.Usuario", new[] { "CiudadId" });
            DropIndex("dbo.Usuario", new[] { "PaisId" });
            DropIndex("dbo.Usuario", new[] { "AreaEspecialidadId" });
            DropIndex("dbo.Usuario", new[] { "GrupoUsuarioId" });
            DropIndex("dbo.Usuario", new[] { "EmpresaId" });
            DropIndex("dbo.AreaEspecialidad", new[] { "AreaGeneralId" });
            DropTable("dbo.AreaGeneral");
            DropTable("dbo.UsuarioContrasena");
            DropTable("dbo.Servicio");
            DropTable("dbo.TransaccionPendienteLinea");
            DropTable("dbo.TransaccionPendiente");
            DropTable("dbo.TransaccionPendientePago");
            DropTable("dbo.TransaccionPago");
            DropTable("dbo.Pago");
            DropTable("dbo.Permiso");
            DropTable("dbo.GrupoPermiso");
            DropTable("dbo.GrupoUsuario");
            DropTable("dbo.Pais");
            DropTable("dbo.Ciudad");
            DropTable("dbo.Edificio");
            DropTable("dbo.EdificioNivel");
            DropTable("dbo.Habitacion");
            DropTable("dbo.HabitacionCliente");
            DropTable("dbo.ClienteDocumento");
            DropTable("dbo.ClienteHistorial");
            DropTable("dbo.ClienteContrasena");
            DropTable("dbo.Cliente");
            DropTable("dbo.Cita");
            DropTable("dbo.Empresa");
            DropTable("dbo.Producto");
            DropTable("dbo.Impuesto");
            DropTable("dbo.TransaccionLinea");
            DropTable("dbo.Transaccion");
            DropTable("dbo.Usuario");
            DropTable("dbo.AreaEspecialidad");
        }
    }
}
