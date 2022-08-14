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
    public class EdificioController : Controller
    {
        private MainDbContext db = new MainDbContext();

        // GET: Edificio
        public async Task<ActionResult> Index()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Habitaciones, Session))
            {
                this.AddNotification("No posees permisos para el Listado de Edificios.", NotificationType.WARNING);
                return RedirectToAction("Index", "DashBoard");
            }
            var edificio = db.Edificio.Include(e => e._Empresa);
            return View(await edificio.OrderBy(a => a.Descripcion).ToListAsync());
        }

        // GET: Edificio/Create
        public ActionResult Create()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Habitaciones, Session))
            {
                this.AddNotification("No posees permisos para Crear Edificios.", NotificationType.WARNING);
                return RedirectToAction("Index", "Edificio");
            }
            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Nombre");
            return View();
        }

        // POST: Edificio/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,EmpresaId,Descripcion,FechaCreacion,FechaModificacion,Estado,Eliminado")] Edificio edificio)
        {
            if (ModelState.IsValid)
            {
                db.Edificio.Add(edificio);
                await db.SaveChangesAsync();
                this.AddNotification("Edificio registrado exitosamente.", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }

            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Nombre", edificio.EmpresaId);
            this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);
            return View(edificio);
        }

        // GET: Edificio/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Habitaciones, Session))
            {
                this.AddNotification("No posees permisos para Editar Edificios.", NotificationType.WARNING);
                return RedirectToAction("Index", "Edificio");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Edificio edificio = await db.Edificio.FindAsync(id);
            if (edificio == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Nombre", edificio.EmpresaId);
            return View(edificio);
        }

        // POST: Edificio/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,EmpresaId,Descripcion,FechaCreacion,FechaModificacion,Estado,Eliminado")] Edificio edificio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(edificio).State = EntityState.Modified;
                await db.SaveChangesAsync();
                this.AddNotification("Edificio modificado exitosamente.", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }
            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Nombre", edificio.EmpresaId);
            this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);
            return View(edificio);
        }

        // GET: Edificio/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Habitaciones, Session))
            {
                this.AddNotification("No posees permisos para Eliminar Edificios.", NotificationType.WARNING);
                return RedirectToAction("Index", "Edificio");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Edificio edificio = await db.Edificio.FindAsync(id);
            if (edificio == null)
            {
                return HttpNotFound();
            }
            return View(edificio);
        }

        // POST: Edificio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Edificio edificio = await db.Edificio.FindAsync(id);
            edificio.FechaModificacion = DateTime.Now;
            edificio.Eliminado = true;
            edificio.Estado = Models.Enums.EstadoEnum.Inactivo;
            db.Entry(edificio).State = EntityState.Modified;
            await db.SaveChangesAsync();
            this.AddNotification("Edificio eliminado exitosamente", NotificationType.SUCCESS);
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
