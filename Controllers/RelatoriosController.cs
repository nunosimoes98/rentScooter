using eRecarga.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eRecarga.Controllers
{
    public class RelatoriosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        [Authorize(Roles = ERecargaRoles.Admin)]
        public ActionResult SelectDistrito()
        {
            ViewBag.DistritoId = new SelectList(db.Distritos, "Id", "Nome");
            return View();
        }

        [Authorize(Roles = ERecargaRoles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectDistrito(int DistritoId)
        {
            return RedirectToAction("SelectEstacao", new { distritoId = DistritoId });
        }

        [Authorize(Roles = ERecargaRoles.Admin)]
        public ActionResult SelectEstacao(int distritoId)
        {
            ViewBag.Distrito = db.Distritos.Find(distritoId);
            ViewBag.EstacaoId = new SelectList(db.Estacoes.Where(e => e.DistritoId == distritoId), "Id", "Nome");
            return View();
        }

        [Authorize(Roles = ERecargaRoles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectEstacao(int EstacaoId, DateTime data)
        {
            return RedirectToAction("List", new { Data = data, EstacaoId = EstacaoId });
        }

        // GET: Relatorios
        [Authorize(Roles = ERecargaRoles.Admin)]
        public ActionResult List(int EstacaoId, DateTime Data)
        {
            var reservas = db.Reservas.Where(r => r.EstacaoId == EstacaoId && r.Data.Year == Data.Year && r.Data.Month == Data.Month && r.Data.Day == Data.Day).OrderBy(r => r.Data);
            List<Dados> dados = new List<Dados>();
            for(int i = 0; i < 24; i++)
            {
                dados.Add(new Dados() { hora = new DateTime(2019, 1, 1, i, 0, 0), valor = 0 });
                dados.Add(new Dados() { hora = new DateTime(2019, 1, 1, i, 30, 0), valor = 0 });
            }

            foreach(var r in reservas)
            {
                foreach(var dado in dados)
                {
                    if(dado.hora.Hour == r.Data.Hour && dado.hora.Minute == r.Data.Minute)
                    {
                        dado.valor++;
                    }
                }
            }

            ViewBag.Dados = dados;
            ViewBag.Estacao = db.Estacoes.Find(EstacaoId);

            return View();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}