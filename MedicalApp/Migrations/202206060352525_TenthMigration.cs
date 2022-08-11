namespace MedicalApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TenthMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UsuarioHorario",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UsuarioId = c.Int(nullable: false),
                        Dia = c.Int(nullable: false),
                        HorarioId = c.Int(nullable: false),
                        CantidadPacientes = c.Int(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Horario", t => t.HorarioId, cascadeDelete: true)
                .ForeignKey("dbo.Usuario", t => t.UsuarioId)
                .Index(t => t.UsuarioId)
                .Index(t => t.HorarioId);
            
            CreateTable(
                "dbo.Horario",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Hora = c.Int(nullable: false),
                        Minutos = c.Int(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UsuarioLicencia",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UsuarioId = c.Int(nullable: false),
                        FechaLicencia = c.DateTime(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuario", t => t.UsuarioId)
                .Index(t => t.UsuarioId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsuarioLicencia", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.UsuarioHorario", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.UsuarioHorario", "HorarioId", "dbo.Horario");
            DropIndex("dbo.UsuarioLicencia", new[] { "UsuarioId" });
            DropIndex("dbo.UsuarioHorario", new[] { "HorarioId" });
            DropIndex("dbo.UsuarioHorario", new[] { "UsuarioId" });
            DropTable("dbo.UsuarioLicencia");
            DropTable("dbo.Horario");
            DropTable("dbo.UsuarioHorario");
        }
    }
}
