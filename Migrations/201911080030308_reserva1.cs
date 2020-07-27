namespace eRecarga.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reserva1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservas", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Reservas", "UserId");
            AddForeignKey("dbo.Reservas", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservas", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Reservas", new[] { "UserId" });
            DropColumn("dbo.Reservas", "UserId");
        }
    }
}
