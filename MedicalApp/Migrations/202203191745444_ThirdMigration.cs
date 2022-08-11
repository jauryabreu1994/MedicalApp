namespace MedicalApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThirdMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UsuarioAsociado",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UsuarioId = c.Int(nullable: false),
                        AsociadoId = c.Int(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                        _Usuario_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuario", t => t._Usuario_Id)
                .ForeignKey("dbo.Usuario", t => t.AsociadoId)
                .Index(t => t.AsociadoId)
                .Index(t => t._Usuario_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsuarioAsociado", "AsociadoId", "dbo.Usuario");
            DropForeignKey("dbo.UsuarioAsociado", "_Usuario_Id", "dbo.Usuario");
            DropIndex("dbo.UsuarioAsociado", new[] { "_Usuario_Id" });
            DropIndex("dbo.UsuarioAsociado", new[] { "AsociadoId" });
            DropTable("dbo.UsuarioAsociado");
        }
    }
}
