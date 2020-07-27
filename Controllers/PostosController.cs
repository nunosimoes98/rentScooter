using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eRecarga.Models;

namespace eRecarga.Controllers
{
    public class PostosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private bool ExistemPostosComMesmoNome(Posto posto)
        {
            var PostoComMesmoNome = db.Postos.FirstOrDefault(x => x.EstacaoId == posto.EstacaoId && x.Nome.Equals(posto.Nome));
            if (PostoComMesmoNome == null)
            {
                return false;
            }
            else if (PostoComMesmoNome.Id != posto.Id) {
                // É o mesmo posto.
                return true;
            }
            return false;
        }

        // GET: Postos
        [Authorize(Roles = ERecargaRoles.Admin)]
        public ActionResult Index()
        {
            var postos = db.Postos.Include(p => p.Estacao);
            return View(postos.ToList());
        }

        // GET: Postos/Details/5
        [Authorize(Roles = ERecargaRoles.Admin)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posto posto = db.Postos.Find(id);
            if (posto == null)
            {
                return HttpNotFound();
            }
            return View(posto);
        }

        // GET: Postos/Create
        [Authorize(Roles = ERecargaRoles.Admin)]
        public ActionResult Create()
        {
            ViewBag.EstacaoId = new SelectList(db.Estacoes, "Id", "Nome");
            return View();
        }

        // GET: Postos/Create/5
        [Authorize(Roles = ERecargaRoles.Admin)]
        public ActionResult CreateComEstacao(int estacaoId)
        {
            ViewBag.Estacao = db.Estacoes.Find(estacaoId);
            return View();
        }

        // POST: Postos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ERecargaRoles.Admin)]
        public ActionResult Create([Bind(Include = "Id,Nome,EstacaoId")] Posto posto)
        {
            if (ModelState.IsValid)
            {
                if (!ExistemPostosComMesmoNome(posto))
                {
                    // Não há postos com o mesmo nome
                    db.Postos.Add(posto);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Já existe um posto com esse nome");
            }

            ViewBag.EstacaoId = new SelectList(db.Estacoes, "Id", "Nome", posto.EstacaoId);
            return View(posto);
        }

        // POST: Postos/Creating/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ERecargaRoles.Admin)]
        public ActionResult CreateComEstacao([Bind(Include = "Id,Nome,EstacaoId")] Posto posto)
        {
            if (ModelState.IsValid)
            {
                if (!ExistemPostosComMesmoNome(posto))
                {
                    // Não há postos com o mesmo nome
                    db.Postos.Add(posto);
                    db.SaveChanges();
                    return RedirectToAction("Details", "Estacoes", new { id = posto.EstacaoId });
                }
                ModelState.AddModelError("", "Já existe um posto com esse nome");
            }

            ViewBag.Estacao = db.Estacoes.Find(posto.EstacaoId);
            return View(posto);
        }

        // GET: Postos/Edit/5
        [Authorize(Roles = ERecargaRoles.Admin)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posto posto = db.Postos.Include(p => p.Estacao).Single(p => p.Id == id);
            if (posto == null)
            {
                return HttpNotFound();
            }
            return View(posto);
        }

        // POST: Postos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ERecargaRoles.Admin)]
        public ActionResult Edit([Bind(Include = "Id,Nome,EstacaoId")] Posto posto)
        {
            if (ModelState.IsValid)
            {
                if (!ExistemPostosComMesmoNome(posto))
                {
                    db.Entry(posto).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", "Estacoes", new { id = posto.EstacaoId });
                }
                ModelState.AddModelError("", "Já existe um posto com esse nome");
            }

            posto.Estacao = db.Estacoes.Find(posto.EstacaoId);
            return View(posto);
        }

        // GET: Postos/Delete/5
        [Authorize(Roles = ERecargaRoles.Admin)]
        public ActionResult Delete(int? id, int? estacaoId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posto posto = db.Postos.Find(id);
            if (posto == null)
            {
                return HttpNotFound();
            }
            ViewBag.EstacaoId = estacaoId;
            return View(posto);
        }

        // POST: Postos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ERecargaRoles.Admin)]
        public ActionResult DeleteConfirmed(int id, int? estacaoId)
        {
            Posto posto = db.Postos.Find(id);
            db.Postos.Remove(posto);
            db.SaveChanges();
            if (estacaoId == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Details", "Estacoes", new { id = estacaoId });
            }
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
