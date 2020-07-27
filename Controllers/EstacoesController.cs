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
    public class EstacoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        public ActionResult IndexUser()
        {
            var estacoes = db.Estacoes.Include(e => e.Distrito).Include(e => e.Postos);
            return View(estacoes.ToList());
        }

        // GET: Estacoes
        [Authorize(Roles = ERecargaRoles.Admin)]
        public ActionResult Index()
        {
            var estacoes = db.Estacoes.Include(e => e.Distrito).Include(e => e.Postos);
            return View(estacoes.ToList());
        }

        // GET: Estacoes/Details/5
        [Authorize(Roles = ERecargaRoles.Admin)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estacao estacao = db.Estacoes.Include(e => e.Postos)
                                         .Include(e => e.Precos)
                                         .SingleOrDefault(e => e.Id == id);
            
            if (estacao == null)
            {
                return HttpNotFound();
            }
            return View(estacao);
        }

        [AllowAnonymous]
        public ActionResult DetailsUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estacao estacao = db.Estacoes.Include(e => e.Postos)
                                         .Include(e => e.Precos)
                                         .SingleOrDefault(e => e.Id == id);

            if (estacao == null)
            {
                return HttpNotFound();
            }
            return View(estacao);
        }

        // GET: Estacoes/Create
        [Authorize(Roles = ERecargaRoles.Admin)]
        public ActionResult Create()
        {
            ViewBag.DistritoId = new SelectList(db.Distritos, "Id", "Nome");
            return View();
        }

        // POST: Estacoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ERecargaRoles.Admin)]
        public ActionResult Create([Bind(Include = "Id,Nome,HoraAbertura,HoraFecho,Longitude,Latitude,DistritoId")] Estacao estacao)
        {
            if (ModelState.IsValid)
            {
                db.Estacoes.Add(estacao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DistritoId = new SelectList(db.Distritos, "Id", "Nome", estacao.DistritoId);
            return View(estacao);
        }

        // GET: Estacoes/Edit/5
        [Authorize(Roles = ERecargaRoles.Admin)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estacao estacao = db.Estacoes.Find(id);
            if (estacao == null)
            {
                return HttpNotFound();
            }
            ViewBag.DistritoId = new SelectList(db.Distritos, "Id", "Nome", estacao.DistritoId);
            return View(estacao);
        }

        // POST: Estacoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ERecargaRoles.Admin)]
        public ActionResult Edit([Bind(Include = "Id,Nome,HoraAbertura,HoraFecho,Longitude,Latitude,DistritoId")] Estacao estacao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estacao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DistritoId = new SelectList(db.Distritos, "Id", "Nome", estacao.DistritoId);
            return View(estacao);
        }

        // GET: Estacoes/Delete/5
        [Authorize(Roles = ERecargaRoles.Admin)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estacao estacao = db.Estacoes.Find(id);
            if (estacao == null)
            {
                return HttpNotFound();
            }
            return View(estacao);
        }

        // POST: Estacoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ERecargaRoles.Admin)]
        public ActionResult DeleteConfirmed(int id)
        {
            Estacao estacao = db.Estacoes.Find(id);
            db.Estacoes.Remove(estacao);
            db.SaveChanges();
            return RedirectToAction("Index");
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
