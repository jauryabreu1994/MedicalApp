namespace MedicalApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeventhMigration : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.HabitacionCliente", new[] { "_Cajero_Id" });
            DropIndex("dbo.HabitacionCliente", new[] { "_Doctor_Id" });
            DropIndex("dbo.HabitacionCliente", new[] { "_Enfermera_Id" });
            DropColumn("dbo.HabitacionCliente", "CajeroId");
            DropColumn("dbo.HabitacionCliente", "DoctorId");
            DropColumn("dbo.HabitacionCliente", "EnfermeraId");
            RenameColumn(table: "dbo.HabitacionCliente", name: "_Cajero_Id", newName: "CajeroId");
            RenameColumn(table: "dbo.HabitacionCliente", name: "_Doctor_Id", newName: "DoctorId");
            RenameColumn(table: "dbo.HabitacionCliente", name: "_Enfermera_Id", newName: "EnfermeraId");
            AlterColumn("dbo.HabitacionCliente", "CajeroId", c => c.Int(nullable: false));
            AlterColumn("dbo.HabitacionCliente", "DoctorId", c => c.Int(nullable: false));
            AlterColumn("dbo.HabitacionCliente", "EnfermeraId", c => c.Int(nullable: false));
            CreateIndex("dbo.HabitacionCliente", "CajeroId");
            CreateIndex("dbo.HabitacionCliente", "DoctorId");
            CreateIndex("dbo.HabitacionCliente", "EnfermeraId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.HabitacionCliente", new[] { "EnfermeraId" });
            DropIndex("dbo.HabitacionCliente", new[] { "DoctorId" });
            DropIndex("dbo.HabitacionCliente", new[] { "CajeroId" });
            AlterColumn("dbo.HabitacionCliente", "EnfermeraId", c => c.Int());
            AlterColumn("dbo.HabitacionCliente", "DoctorId", c => c.Int());
            AlterColumn("dbo.HabitacionCliente", "CajeroId", c => c.Int());
            RenameColumn(table: "dbo.HabitacionCliente", name: "EnfermeraId", newName: "_Enfermera_Id");
            RenameColumn(table: "dbo.HabitacionCliente", name: "DoctorId", newName: "_Doctor_Id");
            RenameColumn(table: "dbo.HabitacionCliente", name: "CajeroId", newName: "_Cajero_Id");
            AddColumn("dbo.HabitacionCliente", "EnfermeraId", c => c.Int(nullable: false));
            AddColumn("dbo.HabitacionCliente", "DoctorId", c => c.Int(nullable: false));
            AddColumn("dbo.HabitacionCliente", "CajeroId", c => c.Int(nullable: false));
            CreateIndex("dbo.HabitacionCliente", "_Enfermera_Id");
            CreateIndex("dbo.HabitacionCliente", "_Doctor_Id");
            CreateIndex("dbo.HabitacionCliente", "_Cajero_Id");
        }
    }
}
