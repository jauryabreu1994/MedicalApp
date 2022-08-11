using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MedicalApp.Context;
using MedicalApp.Models.Edificios;
using MedicalApp.Extensions;

namespace MedicalApp.Controllers.Edificios
{
    public class EdificioNivelController : Controller
    {
        private MainDbContext db = new MainDbContext();

        // GET: EdificioNivel
        public async Task<ActionResult> Index()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Habitaciones, Session))
            {
                this.AddNotification("No posees permisos para el Listado de Niveles por Edificio.", NotificationType.WARNING);
                return RedirectToAction("Index", "DashBoard");
            }
            var edificioNivel = db.EdificioNivel.Include(e => e._Edificio);
            return View(await edificioNivel.ToListAsync());
        }

        // GET: EdificioNivel/Create
        public ActionResult Create()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Habitaciones, Session))
            {
                this.AddNotification("No posees permisos para Crear Niveles por Edificio.", NotificationType.WARNING);
                return RedirectToAction("Index", "EdificioNivel");
            }
            ViewBag.EdificioId = new SelectList(db.Edificio, "Id", "Descripcion");
            return View();
        }

        // POST: EdificioNivel/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,EdificioId,Nivel,FechaCreacion,FechaModificacion,Estado,Eliminado")] EdificioNivel edificioNivel)
        {
            if (ModelState.IsValid)
            {
                db.EdificioNivel.Add(edificioNivel);
                await db.SaveChangesAsync();
                this.AddNotification("Nivel de Edificio registrado exitosamente.", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }

            ViewBag.EdificioId = new SelectList(db.Edificio, "Id", "Descripcion", edificioNivel.EdificioId);
            this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);
            return View(edificioNivel);
        }

        // GET: EdificioNivel/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Habitaciones, Session))
            {
                this.AddNotification("No posees permisos para Editar Niveles por Edificio.", NotificationType.WARNING);
                return RedirectToAction("Index", "EdificioNivel");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EdificioNivel edificioNivel = await db.EdificioNivel.FindAsync(id);
            if (edificioNivel == null)
            {
                return HttpNotFound();
            }
            ViewBag.EdificioId = new SelectList(db.Edificio, "Id", "Descripcion", edificioNivel.EdificioId);
            this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);
            return View(edificioNivel);
        }

        // POST: EdificioNivel/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,EdificioId,Nivel,FechaCreacion,FechaModificacion,Estado,Eliminado")] EdificioNivel edificioNivel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(edificioNivel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                this.AddNotification("Nivel de Edificio modificado exitosamente.", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }
            ViewBag.EdificioId = new SelectList(db.Edificio, "Id", "Descripcion", edificioNivel.EdificioId);
            return View(edificioNivel);
        }

        // GET: EdificioNivel/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Habitaciones, Session))
            {
                this.AddNotification("No posees permisos para Eliminar Niveles por Edificio.", NotificationType.WARNING);
                return RedirectToAction("Index", "EdificioNivel");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EdificioNivel edificioNivel = await db.EdificioNivel.FindAsync(id);
            if (edificioNivel == null)
            {
                return HttpNotFound();
            }
            return View(edificioNivel);
        }

        // POST: EdificioNivel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            EdificioNivel edificioNivel = await db.EdificioNivel.FindAsync(id);
            edificioNivel.FechaModificacion = DateTime.Now;
            edificioNivel.Eliminado = true;
            edificioNivel.Estado = Models.Enums.EstadoEnum.Inactivo;
            db.Entry(edificioNivel).State = EntityState.Modified;
            await db.SaveChangesAsync();
            this.AddNotification("Nivel del Edificio eliminado exitosamente", NotificationType.SUCCESS);
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
