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
    public class PrecosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private bool HoraNaoColideComOutrosPrecos(Preco preco)
        {
            // Compara com as outras hoars da estação.
            List<Preco> precos = db.Precos.Where(p => p.EstacaoId == preco.EstacaoId && p.Id != preco.Id).ToList();
            foreach (Preco p in precos)
            {
                if (p.HoraInicio < preco.HoraInicio && p.HoraFim > preco.HoraInicio)
                    return false;
                if (p.HoraInicio < preco.HoraFim && p.HoraFim > preco.HoraFim)
                    return false;
                if (p.HoraInicio < preco.HoraFim && p.HoraFim > preco.HoraInicio)
                    return false;
            }
            return true;
        }


        // Valida se as horas do preço naõ se sobrepõem a outras horas do mesmo posto
        private bool HoraValida(Preco preco)
        {
            // Validar se a hora é valida
            return preco.HoraInicio < preco.HoraFim;
        }

        // GET: Precos/Create
        [Authorize(Roles = ERecargaRoles.Admin)]
        public ActionResult Create(int? estacaoId)
        {
            ViewBag.Estacao = db.Estacoes.Find(estacaoId);
            return View();
        }

        // POST: Precos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ERecargaRoles.Admin)]
        public ActionResult Create([Bind(Include = "Id,Nome,Valor,HoraInicio,HoraFim,EstacaoId")] Preco preco)
        {
            if (ModelState.IsValid)
            {
                if (HoraValida(preco))
                {
                    if (HoraNaoColideComOutrosPrecos(preco))
                    {
                        db.Precos.Add(preco);
                        db.SaveChanges();
                        return RedirectToAction("Details", "Estacoes", new { id = preco.EstacaoId });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Já existem outros preços nas mesmas horas");
                    }
                }
                else {
                    ModelState.AddModelError("", "A hora de início tem de ser anterior à hora de fim.");
                }
            }

            ViewBag.Estacao = db.Estacoes.Find(preco.EstacaoId);
            return View(preco);
        }

        // GET: Precos/Edit/5
        [Authorize(Roles = ERecargaRoles.Admin)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Preco preco = db.Precos.Find(id);
            if (preco == null)
            {
                return HttpNotFound();
            }
            ViewBag.Estacao = db.Estacoes.Find(preco.EstacaoId);
            return View(preco);
        }

        // POST: Precos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ERecargaRoles.Admin)]
        public ActionResult Edit([Bind(Include = "Id,Nome,Valor,HoraInicio,HoraFim,EstacaoId")] Preco preco)
        {
            if (ModelState.IsValid)
            {
                if (HoraValida(preco))
                {
                    if (HoraNaoColideComOutrosPrecos(preco))
                    {
                        db.Entry(preco).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Details", "Estacoes", new { id = preco.EstacaoId });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Já existem outros preços nas mesmas horas");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "A hora de início tem de ser anterior à hora de fim.");
                }
            }

            ViewBag.Estacao = db.Estacoes.Find(preco.EstacaoId);
            return View(preco);
        }

        // GET: Precos/Delete/5
        [Authorize(Roles = ERecargaRoles.Admin)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Preco preco = db.Precos.Find(id);
            if (preco == null)
            {
                return HttpNotFound();
            }
            ViewBag.Estacao = db.Estacoes.Find(preco.EstacaoId);
            return View(preco);
        }

        // POST: Precos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ERecargaRoles.Admin)]
        public ActionResult DeleteConfirmed(int id)
        {
            Preco preco = db.Precos.Find(id);
            int estacaoId = preco.EstacaoId;
            db.Precos.Remove(preco);
            db.SaveChanges();
            return RedirectToAction("Details", "Estacoes", new { id = estacaoId});
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
