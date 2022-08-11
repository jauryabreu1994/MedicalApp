using MedicalApp.Models.Citas;
using MedicalApp.Models.Clientes;
using MedicalApp.Models.Edificios;
using MedicalApp.Models.Empresas;
using MedicalApp.Models.Horarios;
using MedicalApp.Models.Impuestos;
using MedicalApp.Models.MediosDePago;
using MedicalApp.Models.Productos;
using MedicalApp.Models.Servicios;
using MedicalApp.Models.Ubicaciones;
using MedicalApp.Models.Usuarios;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace MedicalApp.Context
{
    public class MainDbContext : DbContext
    {

        public MainDbContext() : base("name=mssql_web")
        { }

        public DbSet<Cita> Cita { get; set; }

        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<ClienteContrasena> ClienteContrasena { get; set; }
        public DbSet<ClienteDocumento> ClienteDocumento { get; set; }
        public DbSet<ClienteHistorial> ClienteHistorial { get; set; }


        public DbSet<Edificio> Edificio { get; set; }
        public DbSet<EdificioNivel> EdificioNivel { get; set; }
        public DbSet<Habitacion> Habitacion { get; set; }
        public DbSet<HabitacionCliente> HabitacionCliente { get; set; }

        public DbSet<Empresa> Empresa { get; set; }

        public DbSet<Impuesto> Impuesto { get; set; }

        public DbSet<Pago> Pago { get; set; }

        public DbSet<Producto> Producto { get; set; }
        public DbSet<ProductoCliente> ProductoCliente { get; set; }

        public DbSet<Servicio> Servicio { get; set; }

        public DbSet<Ciudad> Ciudad { get; set; }
        public DbSet<Pais> Pais { get; set; }

        public DbSet<AreaEspecialidad> AreaEspecialidad { get; set; }
        public DbSet<AreaGeneral> AreaGeneral { get; set; }
        public DbSet<GrupoPermiso> GrupoPermiso { get; set; }
        public DbSet<GrupoUsuario> GrupoUsuario { get; set; }
        public DbSet<Permiso> Permiso { get; set; }
        public DbSet<UsuarioAsociado> UsuarioAsociado { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<UsuarioContrasena> UsuarioContrasena { get; set; }
        public DbSet<UsuarioHorario> UsuarioHorario { get; set; }
        public DbSet<UsuarioLicencia> UsuarioLicencia { get; set; }

        public DbSet<Horario> Horario { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region Common Entities
            modelBuilder.Properties<int>()
                .Where(a => a.Name == "Id")
                .Configure(c => c.HasColumnType("int").HasColumnName("Id")
                .IsKey()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity));
            #endregion

            modelBuilder.Entity<Cita>().ToTable("Cita", "dbo").HasKey(a => a.Id);

            modelBuilder.Entity<Horario>().ToTable("Horario", "dbo").HasKey(a => a.Id);

            modelBuilder.Entity<Cliente>().ToTable("Cliente", "dbo").HasKey(a => a.Id);
            modelBuilder.Entity<ClienteContrasena>().ToTable("ClienteContrasena", "dbo").HasKey(a => a.Id);
            modelBuilder.Entity<ClienteDocumento>().ToTable("ClienteDocumento", "dbo").HasKey(a => a.Id);
            modelBuilder.Entity<ClienteHistorial>().ToTable("ClienteHistorial", "dbo").HasKey(a => a.Id);

            modelBuilder.Entity<Edificio>().ToTable("Edificio", "dbo").HasKey(a => a.Id);
            modelBuilder.Entity<EdificioNivel>().ToTable("EdificioNivel", "dbo").HasKey(a => a.Id);
            modelBuilder.Entity<Habitacion>().ToTable("Habitacion", "dbo").HasKey(a => a.Id);
            modelBuilder.Entity<HabitacionCliente>().ToTable("HabitacionCliente", "dbo").HasKey(a => a.Id);

            modelBuilder.Entity<Empresa>().ToTable("Empresa", "dbo").HasKey(a => a.Id);

            modelBuilder.Entity<Impuesto>().ToTable("Impuesto", "dbo").HasKey(a => a.Id);

            modelBuilder.Entity<Pago>().ToTable("Pago", "dbo").HasKey(a => a.Id);

            modelBuilder.Entity<Producto>().ToTable("Producto", "dbo").HasKey(a => a.Id);

            modelBuilder.Entity<Servicio>().ToTable("Servicio", "dbo").HasKey(a => a.Id);

            modelBuilder.Entity<Ciudad>().ToTable("Ciudad", "dbo").HasKey(a => a.Id);
            modelBuilder.Entity<Pais>().ToTable("Pais", "dbo").HasKey(a => a.Id);

            modelBuilder.Entity<AreaEspecialidad>().ToTable("AreaEspecialidad", "dbo").HasKey(a => a.Id);
            modelBuilder.Entity<AreaGeneral>().ToTable("AreaGeneral", "dbo").HasKey(a => a.Id);
            modelBuilder.Entity<GrupoPermiso>().ToTable("GrupoPermiso", "dbo").HasKey(a => a.Id);
            modelBuilder.Entity<GrupoUsuario>().ToTable("GrupoUsuario", "dbo").HasKey(a => a.Id);
            modelBuilder.Entity<Permiso>().ToTable("Permiso", "dbo").HasKey(a => a.Id);
            modelBuilder.Entity<UsuarioAsociado>().ToTable("UsuarioAsociado", "dbo").HasKey(a => a.Id);
            modelBuilder.Entity<Usuario>().ToTable("Usuario", "dbo").HasKey(a => a.Id);
            modelBuilder.Entity<UsuarioContrasena>().ToTable("UsuarioContrasena", "dbo").HasKey(a => a.Id);
            modelBuilder.Entity<UsuarioHorario>().ToTable("UsuarioHorario", "dbo").HasKey(a => a.Id);
            modelBuilder.Entity<UsuarioLicencia>().ToTable("UsuarioLicencia", "dbo").HasKey(a => a.Id);

            #region Empresa
            modelBuilder.Entity<Empresa>()
                .HasMany(a => a.__Citas)
                .WithRequired(b => b._Empresa)
                .HasForeignKey(c => c.EmpresaId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Empresa>()
                .HasMany(a => a.__Pagos)
                .WithRequired(b => b._Empresa)
                .HasForeignKey(c => c.EmpresaId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Empresa>()
                .HasMany(a => a.__Usuarios)
                .WithRequired(b => b._Empresa)
                .HasForeignKey(c => c.EmpresaId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Empresa>()
                .HasMany(a => a.__Edificios)
                .WithRequired(b => b._Empresa)
                .HasForeignKey(c => c.EmpresaId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Empresa>()
                .HasMany(a => a.__Productos)
                .WithRequired(b => b._Empresa)
                .HasForeignKey(c => c.EmpresaId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Empresa>()
                .HasMany(a => a.__Servicios)
                .WithRequired(b => b._Empresa)
                .HasForeignKey(c => c.EmpresaId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Empresa>()
                .HasMany(a => a.__Impuestos)
                .WithRequired(b => b._Empresa)
                .HasForeignKey(c => c.EmpresaId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Empresa>()
                .HasMany(a => a.__GrupoUsuarios)
                .WithRequired(b => b._Empresa)
                .HasForeignKey(c => c.EmpresaId).WillCascadeOnDelete(false);

            #endregion

            #region Cliente
            modelBuilder.Entity<Cliente>()
                .HasMany(a => a.__Citas)
                .WithRequired(b => b._Cliente)
                .HasForeignKey(c => c.ClienteId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Cliente>()
                .HasMany(a => a.__HabitacionClientes)
                .WithRequired(b => b._Cliente)
                .HasForeignKey(c => c.ClienteId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Cliente>()
                .HasMany(a => a.__ClienteHistoriales)
                .WithRequired(b => b._Cliente)
                .HasForeignKey(c => c.ClienteId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Cliente>()
                .HasMany(a => a.__ClienteContrasenas)
                .WithRequired(b => b._Cliente)
                .HasForeignKey(c => c.ClienteId).WillCascadeOnDelete(false);
            #endregion

            #region ClienteHistorial
            modelBuilder.Entity<ClienteHistorial>()
                .HasMany(a => a.__ClienteDocumentos)
                .WithRequired(b => b._ClienteHistorial)
                .HasForeignKey(c => c.ClienteHistorialId).WillCascadeOnDelete(false);
            #endregion

            #region Pais
            modelBuilder.Entity<Pais>()
                .HasMany(a => a.__Ciudades)
                .WithRequired(b => b._Pais)
                .HasForeignKey(c => c.PaisId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Pais>()
                .HasMany(a => a.__Clientes)
                .WithRequired(b => b._Pais)
                .HasForeignKey(c => c.PaisId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Pais>()
                .HasMany(a => a.__Empresas)
                .WithRequired(b => b._Pais)
                .HasForeignKey(c => c.PaisId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Pais>()
                .HasMany(a => a.__Usuarios)
                .WithRequired(b => b._Pais)
                .HasForeignKey(c => c.PaisId).WillCascadeOnDelete(false); 
            #endregion

            #region Ciudad
            modelBuilder.Entity<Ciudad>()
                .HasMany(a => a.__Clientes)
                .WithRequired(b => b._Ciudad)
                .HasForeignKey(c => c.CiudadId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Ciudad>()
                .HasMany(a => a.__Empresas)
                .WithRequired(b => b._Ciudad)
                .HasForeignKey(c => c.CiudadId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Ciudad>()
                .HasMany(a => a.__Usuarios)
                .WithRequired(b => b._Ciudad)
                .HasForeignKey(c => c.CiudadId).WillCascadeOnDelete(false);
            #endregion

            #region Habitacion
            modelBuilder.Entity<Habitacion>()
                .HasMany(a => a.__HabitacionClientes)
                .WithRequired(b => b._Habitacion)
                .HasForeignKey(c => c.HabitacionId).WillCascadeOnDelete(false);
            #endregion

            #region EdificioNivel
            modelBuilder.Entity<EdificioNivel>()
                .HasMany(a => a.__Habitaciones)
                .WithRequired(b => b._EdificioNivel)
                .HasForeignKey(c => c.EdificioNivelId).WillCascadeOnDelete(false);
            #endregion

            #region Edificio
            modelBuilder.Entity<Edificio>()
                .HasMany(a => a.__EdificioNiveles)
                .WithRequired(b => b._Edificio)
                .HasForeignKey(c => c.EdificioId).WillCascadeOnDelete(false);
            #endregion

            #region Impuesto
            modelBuilder.Entity<Impuesto>()
                .HasMany(a => a.__Servicios)
                .WithRequired(b => b._Impuesto)
                .HasForeignKey(c => c.ImpuestoId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Impuesto>()
                .HasMany(a => a.__Productos)
                .WithRequired(b => b._Impuesto)
                .HasForeignKey(c => c.ImpuestoId).WillCascadeOnDelete(false);

            #endregion

            #region Usuarios
            modelBuilder.Entity<Usuario>()
                .HasMany(a => a.__UsuarioContrasenas)
                .WithRequired(b => b._Usuario)
                .HasForeignKey(c => c.UsuarioId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
               .HasMany(a => a.__UsuarioDoctorAsociados)
               .WithRequired(b => b._Doctor)
               .HasForeignKey(c => c.DoctorId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
               .HasMany(a => a.__UsuarioAsistenteAsociados)
               .WithRequired(b => b._Asistente)
               .HasForeignKey(c => c.AsistenteId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
               .HasMany(a => a.__CajerosHabitaciones)
               .WithRequired(b => b._Cajero)
               .HasForeignKey(c => c.CajeroId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
              .HasMany(a => a.__DoctoresHabitaciones)
              .WithRequired(b => b._Doctor)
              .HasForeignKey(c => c.DoctorId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
              .HasMany(a => a.__EnfermerosHabitaciones)
              .WithRequired(b => b._Enfermera)
              .HasForeignKey(c => c.EnfermeraId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
              .HasMany(a => a.__UsuariosLicencias)
              .WithRequired(b => b._Usuario)
              .HasForeignKey(c => c.UsuarioId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
              .HasMany(a => a.__UsuariosHorarios)
              .WithRequired(b => b._Doctor)
              .HasForeignKey(c => c.UsuarioId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
              .HasMany(a => a.__Citas)
              .WithRequired(b => b._Usuario)
              .HasForeignKey(c => c.UsuarioId).WillCascadeOnDelete(false);
            #endregion

            #region Área General
            modelBuilder.Entity<AreaGeneral>()
                .HasMany(a => a.__AreaEspecialidades)
                .WithRequired(b => b._AreaGeneral)
                .HasForeignKey(c => c.AreaGeneralId).WillCascadeOnDelete(false);
            #endregion

            #region Area Especialidad
            modelBuilder.Entity<AreaEspecialidad>()
                 .HasMany(a => a.__Usuarios)
                 .WithRequired(b => b._AreaEspecialidad)
                 .HasForeignKey(c => c.AreaEspecialidadId).WillCascadeOnDelete(false);
            #endregion

            #region Grupo Usuario
            modelBuilder.Entity<GrupoUsuario>()
                .HasMany(a => a.__GrupoPermisos)
                .WithRequired(b => b._GrupoUsuario)
                .HasForeignKey(c => c.GrupoUsuarioId).WillCascadeOnDelete(false);

            modelBuilder.Entity<GrupoUsuario>()
                .HasMany(a => a.__Usuarios)
                .WithRequired(b => b._GrupoUsuario)
                .HasForeignKey(c => c.GrupoUsuarioId).WillCascadeOnDelete(false);
            #endregion

            #region Permiso
            modelBuilder.Entity<Permiso>()
                .HasMany(a => a.__GrupoPermisos)
                .WithRequired(b => b._Permiso)
                .HasForeignKey(c => c.PermisoId).WillCascadeOnDelete(false);
            #endregion

            #region Producto
            modelBuilder.Entity<Producto>()
                .HasMany(a => a.__ProductoCliente)
                .WithRequired(b => b._Producto)
                .HasForeignKey(c => c.ProductoId).WillCascadeOnDelete(false);
            #endregion

            #region Habitacion Cliente
            modelBuilder.Entity<HabitacionCliente>()
                .HasMany(a => a.__ProductoCliente)
                .WithRequired(b => b._HabitacionCliente)
                .HasForeignKey(c => c.HabitacionClienteId).WillCascadeOnDelete(false);
            #endregion


        }
    }
}