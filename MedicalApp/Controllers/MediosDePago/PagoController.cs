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
using MedicalApp.Models.MediosDePago;
using MedicalApp.Extensions;

namespace MedicalApp.Controllers.MediosDePago
{
    public class PagoController : Controller
    {
        private MainDbContext db = new MainDbContext();

        // GET: Pago
        public async Task<ActionResult> Index()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Servicios, Session))
            {
                this.AddNotification("No posees permisos para el Listado de Pagos.", NotificationType.WARNING);
                return RedirectToAction("Index", "DashBoard");
            }
            var pago = db.Pago.OrderBy(a => a.Descripcion).Include(p => p._Empresa);
            return View(await pago.ToListAsync());
        }

        // GET: Pago/Create
        public ActionResult Create()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Servicios, Session))
            {
                this.AddNotification("No posees permisos para Crear Pagos.", NotificationType.WARNING);
                return RedirectToAction("Index", "Pago");
            }
            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Nombre");
            return View();
        }

        // POST: Pago/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,EmpresaId,Descripcion,TasaCambio,TipoPago,FechaCreacion,FechaModificacion,Estado,Eliminado")] Pago pago)
        {
            if (ModelState.IsValid)
            {
                db.Pago.Add(pago);
                await db.SaveChangesAsync();
                this.AddNotification("Pago registrado exitosamente.", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }

            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Nombre", pago.EmpresaId);
            this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);
            return View(pago);
        }

        // GET: Pago/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Servicios, Session))
            {
                this.AddNotification("No posees permisos para Editar Pagos.", NotificationType.WARNING);
                return RedirectToAction("Index", "Pago");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pago pago = await db.Pago.FindAsync(id);
            if (pago == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Nombre", pago.EmpresaId);
            return View(pago);
        }

        // POST: Pago/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,EmpresaId,Descripcion,TasaCambio,TipoPago,FechaCreacion,FechaModificacion,Estado,Eliminado")] Pago pago)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pago).State = EntityState.Modified;
                await db.SaveChangesAsync();
                this.AddNotification("Pago modificado exitosamente.", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }
            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Nombre", pago.EmpresaId);
            this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);
            return View(pago);
        }

        // GET: Pago/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Servicios, Session))
            {
                this.AddNotification("No posees permisos para Eliminar Pagos.", NotificationType.WARNING);
                return RedirectToAction("Index", "Pago");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pago pago = await db.Pago.FindAsync(id);
            if (pago == null)
            {
                return HttpNotFound();
            }
            return View(pago);
        }

        // POST: Pago/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Pago pago = await db.Pago.FindAsync(id);
            db.Pago.Remove(pago);
            await db.SaveChangesAsync();
            this.AddNotification("Pago eliminado exitosamente.", NotificationType.SUCCESS);
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
