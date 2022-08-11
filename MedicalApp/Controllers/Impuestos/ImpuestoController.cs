using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using MedicalApp.Context;
using MedicalApp.Models.Impuestos;
using System;
using MedicalApp.Extensions;

namespace MedicalApp.Controllers.Impuestos
{
    public class ImpuestoController : Controller
    {
        private MainDbContext db = new MainDbContext();

        // GET: Impuesto
        public async Task<ActionResult> Index()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Citas, Session))
            {
                this.AddNotification("No posees permisos para el Listado de Impuestos.", NotificationType.WARNING);
                return RedirectToAction("Index", "DashBoard");
            }
            var impuesto = db.Impuesto.Include(i => i._Empresa);
            return View(await impuesto.ToListAsync());
        }

        // GET: Impuesto/Create
        public ActionResult Create()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Citas, Session))
            {
                this.AddNotification("No posees permisos para Crear Impuestos.", NotificationType.WARNING);
                return RedirectToAction("Index", "Impuesto");
            }
            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Nombre");
            return View();
        }

        // POST: Impuesto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,EmpresaId,Descripcion,Monto,Prefijo,FechaCreacion,FechaModificacion,Estado,Eliminado")] Impuesto impuesto)
        {
            if (ModelState.IsValid)
            {
                db.Impuesto.Add(impuesto);
                await db.SaveChangesAsync(); 
                this.AddNotification("Impuesto registrado exitosamente.", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }

            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Nombre", impuesto.EmpresaId);
            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Nombre", impuesto.EmpresaId);

            return View(impuesto);
        }

        // GET: Impuesto/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Citas, Session))
            {
                this.AddNotification("No posees permisos para Editar Impuestos.", NotificationType.WARNING);
                return RedirectToAction("Index", "Impuesto");
            }
            if (id == null)
            {
                return RedirectToAction("Error", "Home");;
            }
            Impuesto impuesto = await db.Impuesto.FindAsync(id);
            if (impuesto == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Nombre", impuesto.EmpresaId);
            return View(impuesto);
        }

        // POST: Impuesto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,EmpresaId,Descripcion,Monto,Prefijo,FechaCreacion,FechaModificacion,Estado,Eliminado")] Impuesto impuesto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(impuesto).State = EntityState.Modified;
                await db.SaveChangesAsync();
                this.AddNotification("Impuesto modificado exitosamente.", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }
            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Nombre", impuesto.EmpresaId);
            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Nombre", impuesto.EmpresaId);

            return View(impuesto);
        }

        // GET: Impuesto/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Citas, Session))
            {
                this.AddNotification("No posees permisos para Eliminar Impuestos.", NotificationType.WARNING);
                return RedirectToAction("Index", "Impuesto");
            }
            if (id == null)
            {
                return RedirectToAction("Error", "Home");;
            }
            Impuesto impuesto = await db.Impuesto.FindAsync(id);
            if (impuesto == null)
            {
                return HttpNotFound();
            }
            return View(impuesto);
        }

        // POST: Impuesto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Impuesto impuesto = await db.Impuesto.FindAsync(id);
            impuesto.FechaModificacion = DateTime.Now;
            impuesto.Eliminado = true;
            impuesto.Estado = Models.Enums.EstadoEnum.Inactivo;
            db.Entry(impuesto).State = EntityState.Modified;
            await db.SaveChangesAsync();
            this.AddNotification("Impuesto eliminado exitosamente", NotificationType.SUCCESS);
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
