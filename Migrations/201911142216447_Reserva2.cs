namespace eRecarga.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Reserva2 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Reservas", "EstacaoId");
            AddForeignKey("dbo.Reservas", "EstacaoId", "dbo.Estacaos", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservas", "EstacaoId", "dbo.Estacaos");
            DropIndex("dbo.Reservas", new[] { "EstacaoId" });
        }
    }
}
