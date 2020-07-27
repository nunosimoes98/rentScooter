namespace eRecarga.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReservaData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservas", "Data", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Reservas", "CodigoServico", c => c.String());
            DropColumn("dbo.Reservas", "HoraInicio");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reservas", "HoraInicio", c => c.Time(nullable: false, precision: 7));
            AlterColumn("dbo.Reservas", "CodigoServico", c => c.String(nullable: false));
            DropColumn("dbo.Reservas", "Data");
        }
    }
}
