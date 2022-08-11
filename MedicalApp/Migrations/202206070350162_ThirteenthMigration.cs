namespace MedicalApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThirteenthMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Cita", "UsuarioId", "dbo.Usuario");
            AddForeignKey("dbo.Cita", "UsuarioId", "dbo.Usuario", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cita", "UsuarioId", "dbo.Usuario");
            AddForeignKey("dbo.Cita", "UsuarioId", "dbo.Usuario", "Id", cascadeDelete: true);
        }
    }
}
