namespace MedicalApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SecondMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Producto", "_Usuario_Id", "dbo.Usuario");
            DropIndex("dbo.Producto", new[] { "_Usuario_Id" });
            DropColumn("dbo.Producto", "_Usuario_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Producto", "_Usuario_Id", c => c.Int());
            CreateIndex("dbo.Producto", "_Usuario_Id");
            AddForeignKey("dbo.Producto", "_Usuario_Id", "dbo.Usuario", "Id");
        }
    }
}
