namespace eRecarga.Migrations
{
    using eRecarga.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<eRecarga.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(eRecarga.Models.ApplicationDbContext context)
        {
            context.Distritos.AddOrUpdate(d => d.Nome, new Distrito { Nome = "Aveiro" });
            context.Distritos.AddOrUpdate(d => d.Nome, new Distrito { Nome = "Bragança" });
            context.Distritos.AddOrUpdate(d => d.Nome, new Distrito { Nome = "Beja" });
            context.Distritos.AddOrUpdate(d => d.Nome, new Distrito { Nome = "Braga" });
            context.Distritos.AddOrUpdate(d => d.Nome, new Distrito { Nome = "Coimbra" });
            context.Distritos.AddOrUpdate(d => d.Nome, new Distrito { Nome = "Porto" });
            context.Distritos.AddOrUpdate(d => d.Nome, new Distrito { Nome = "Lisboa" });
            context.Distritos.AddOrUpdate(d => d.Nome, new Distrito { Nome = "Setubal" });
            context.Distritos.AddOrUpdate(d => d.Nome, new Distrito { Nome = "Leiria" });
            context.Distritos.AddOrUpdate(d => d.Nome, new Distrito { Nome = "Santarém" });
            context.Distritos.AddOrUpdate(d => d.Nome, new Distrito { Nome = "Guarda" });
            context.Distritos.AddOrUpdate(d => d.Nome, new Distrito { Nome = "Évora" });
            context.Distritos.AddOrUpdate(d => d.Nome, new Distrito { Nome = "Castelo Branco" });
            context.Distritos.AddOrUpdate(d => d.Nome, new Distrito { Nome = "Portalegre" });
            context.Distritos.AddOrUpdate(d => d.Nome, new Distrito { Nome = "Viana do Castelo" });
            context.Distritos.AddOrUpdate(d => d.Nome, new Distrito { Nome = "Vila Real" });
            context.Distritos.AddOrUpdate(d => d.Nome, new Distrito { Nome = "Faro" });
            context.Distritos.AddOrUpdate(d => d.Nome, new Distrito { Nome = "Viseu" });
        }
    }
}
