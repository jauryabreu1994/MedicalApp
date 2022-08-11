namespace MedicalApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FourthMigration : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.UsuarioAsociado", new[] { "_Usuario_Id" });
            RenameColumn(table: "dbo.UsuarioAsociado", name: "_Usuario_Id", newName: "AsistenteId");
            RenameColumn(table: "dbo.UsuarioAsociado", name: "AsociadoId", newName: "DoctorId");
            RenameIndex(table: "dbo.UsuarioAsociado", name: "IX_AsociadoId", newName: "IX_DoctorId");
            AlterColumn("dbo.UsuarioAsociado", "AsistenteId", c => c.Int(nullable: false));
            CreateIndex("dbo.UsuarioAsociado", "AsistenteId");
            DropColumn("dbo.UsuarioAsociado", "UsuarioId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UsuarioAsociado", "UsuarioId", c => c.Int(nullable: false));
            DropIndex("dbo.UsuarioAsociado", new[] { "AsistenteId" });
            AlterColumn("dbo.UsuarioAsociado", "AsistenteId", c => c.Int());
            RenameIndex(table: "dbo.UsuarioAsociado", name: "IX_DoctorId", newName: "IX_AsociadoId");
            RenameColumn(table: "dbo.UsuarioAsociado", name: "DoctorId", newName: "AsociadoId");
            RenameColumn(table: "dbo.UsuarioAsociado", name: "AsistenteId", newName: "_Usuario_Id");
            CreateIndex("dbo.UsuarioAsociado", "_Usuario_Id");
        }
    }
}
