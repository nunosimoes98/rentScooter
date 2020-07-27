using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eRecarga.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace eRecarga.Controllers
{
    public class ReservasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        private String CriarCodigoServico(Reserva reserva)
        {
            String codigo = "R" + reserva.Data.Year.ToString() + reserva.Data.Month.ToString()
                          + "C" + reserva.Id.ToString();
            return codigo;
        }

        private bool PossivelFazerReserva(Reserva reserva)
        {
            int n = db.Reservas.Count(r => r.EstacaoId == reserva.EstacaoId && r.Data == reserva.Data);
            // validar se há postos suficientes para todas as reservas
            int numero_postos = db.Postos.Count(p => p.EstacaoId == reserva.EstacaoId);
            if(numero_postos <= n)
            {
                return false;
            }

            return true;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [Authorize(Roles = ERecargaRoles.Admin)]
        public ActionResult Estacao(int estacaoId, DateTime? dia)
        {
            if(dia == null)
            {
                dia = DateTime.Now;
            }
            var estacao = db.Estacoes.Include(e => e.Postos).First(e => e.Id == estacaoId);
            if(estacao == null)
            {
                return HttpNotFound();
            }
            ViewBag.Estacao = estacao;

            var reservas = db.Reservas.Include(r => r.Estacao.Precos)
                                      .Where(r => r.EstacaoId == estacaoId && r.Data.Year == dia.Value.Year && r.Data.Month == dia.Value.Month && r.Data.Day == dia.Value.Day )
                                      .OrderBy(r => r.Data);

            SetViewBagHoras(estacao.HoraAbertura, estacao.HoraFecho);

            return View(reservas.ToList());
        }

        // GET: Reservas Apenas as reservas do utilizador
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var reservas = db.Reservas.Include(r => r.Estacao.Precos)
                                      .Where(r => r.UserId == userId);
            return View(reservas.ToList());
        }




        // GET: Reservas/SelectDistrito
        public ActionResult SelectDistrito()
        {
            ViewBag.DistritoId = new SelectList(db.Distritos, "Id", "Nome");
            return View();
        }

        [Authorize(Roles = ERecargaRoles.Admin)]
        public ActionResult ManageSelectDistrito()
        {
            ViewBag.DistritoId = new SelectList(db.Distritos, "Id", "Nome");
            return View();
        }

        [Authorize(Roles = ERecargaRoles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageSelectDistrito(int DistritoId)
        {
            return RedirectToAction("ManageSelectEstacao", new { distritoId = DistritoId });
        }

        [Authorize(Roles = ERecargaRoles.Admin)]
        public ActionResult ManageSelectEstacao(int distritoId)
        {
            SetViewBag(distritoId);
            return View();
        }

        [Authorize(Roles = ERecargaRoles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageSelectEstacao(int DistritoId, int EstacaoId, DateTime data)
        {
            return RedirectToAction("ManageIndex", new { distritoId = DistritoId, estacaoId = EstacaoId, data = data });
        }

        [Authorize(Roles = ERecargaRoles.Admin)]
        public ActionResult ManageIndex(int distritoId, int estacaoId, DateTime data)
        {
            ViewBag.Distrito = db.Distritos.Find(distritoId);
            ViewBag.Estacao = db.Estacoes.Find(estacaoId);
            ViewBag.Reserva = db.Reservas.Where(r => r.EstacaoId == estacaoId && r.Data.Year == data.Year && r.Data.Month == data.Month && data.Day == r.Data.Day)
                                         .Include(r => r.User);
            ViewBag.Data = data;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectDistrito(int DistritoId)
        {
            return RedirectToAction("Create", new { distritoId = DistritoId });
        }

        private void SetViewBag(int distritoId)
        {
            ViewBag.Distrito = db.Distritos.Find(distritoId);
            ViewBag.EstacaoId = new SelectList(db.Estacoes.Where(e => e.DistritoId == distritoId), "Id", "Nome");
        }

        private void SetViewBagHoras(TimeSpan horaInicio, TimeSpan horaFim)
        {
            List<String> horas = new List<string>();

            if (horaInicio.Minutes == 0)
            {
                horas.Add(horaInicio.Hours.ToString("D2") + ":" + "00");
                horas.Add(horaInicio.Hours.ToString("D2") + ":" + "30");
            }
            else
            {
                horas.Add(horaInicio.Hours.ToString("D2") + ":" + "30");
            }

            for (int i = horaInicio.Hours + 1; i < horaFim.Hours; i++)
            {
                horas.Add(i.ToString("D2") + ":" + "00");
                horas.Add(i.ToString("D2") + ":" + "30");
            }

            if (horaFim.Minutes <= 30)
            {
                horas.Add(horaFim.Hours.ToString("D2") + ":" + "00");
            }
            if (horaFim.Minutes > 30)
            {
                horas.Add(horaFim.Hours.ToString("D2") + ":" + "30");
            }

            ViewBag.Hora = new SelectList(horas);
        }


        // GET: Reservas/Create/distritoId
        public ActionResult Create(int distritoId)
        {
            var inicio = new TimeSpan(0, 0, 0);
            var fim = new TimeSpan(23, 59, 0);
            SetViewBagHoras(inicio, fim);
            SetViewBag(distritoId);
            return View();
        }

        // POST: Reservas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EstacaoId,Data")] Reserva reserva, String Hora)
        {
            reserva.UserId = User.Identity.GetUserId();
            int horas = Int32.Parse(Hora.Split(':')[0]);
            int minutos = Int32.Parse(Hora.Split(':')[1]);
            reserva.Data = reserva.Data.AddHours(horas).AddMinutes(minutos);
            
            //ModelState.SetModelValue("UserId", new ValueProviderResult(reserva.UserId, "", CultureInfo.InvariantCulture));
            ModelState["UserId"].Errors.Clear();
            if (ModelState.IsValid)
            {
                if (PossivelFazerReserva(reserva))
                {
                    db.Reservas.Add(reserva);
                    db.SaveChanges();
                    reserva.CodigoServico = CriarCodigoServico(reserva);
                    db.Entry(reserva).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Não há postos disponíveis a essa hora");
                }
            }


            var inicio = new TimeSpan(0, 0, 0);
            var fim = new TimeSpan(23, 59, 0);
            SetViewBagHoras(inicio, fim );
            SetViewBag(db.Estacoes.Find(reserva.EstacaoId).DistritoId);
            return View(reserva);
        }

        // GET: Reservas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reserva reserva = db.Reservas.Find(id);
            if (reserva == null)
            {
                return HttpNotFound();
            }

            if (reserva.UserId != User.Identity.GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            return View(reserva);
        }

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reserva reserva = db.Reservas.Find(id);

            if (reserva.UserId != User.Identity.GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            db.Reservas.Remove(reserva);
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
