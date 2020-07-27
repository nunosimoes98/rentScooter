namespace eRecarga.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CodigoServico : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservas", "CodigoServico", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reservas", "CodigoServico");
        }
    }
}
