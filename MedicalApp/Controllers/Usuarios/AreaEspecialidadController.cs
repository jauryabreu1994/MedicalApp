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
using MedicalApp.Models.Usuarios;
using MedicalApp.Extensions;

namespace MedicalApp.Controllers.Usuarios
{
    public class AreaEspecialidadController : Controller
    {
        private MainDbContext db = new MainDbContext();

        // GET: AreaEspecialidad
        public async Task<ActionResult> Index()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Usuario, Session))
            {
                this.AddNotification("No posees permisos para el Listado de Área de Especialidad.", NotificationType.WARNING);
                return RedirectToAction("Index", "DashBoard");
            }
            var areaEspecialidad = db.AreaEspecialidad.Include(a => a._AreaGeneral);
            return View(await areaEspecialidad.OrderBy(a => a.Descripcion).ToListAsync());
        }

        // GET: AreaEspecialidad/Create
        public ActionResult Create()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Citas, Session))
            {
                this.AddNotification("No posees permisos para Crear Área de Especialidad.", NotificationType.WARNING);
                return RedirectToAction("Index", "AreaEspecialidad");
            }

            ViewBag.AreaGeneralId = new SelectList(db.AreaGeneral, "Id", "Descripcion");
            return View();
        }

        // POST: AreaEspecialidad/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,AreaGeneralId,Descripcion,FechaCreacion,FechaModificacion,Estado,Eliminado")] AreaEspecialidad areaEspecialidad)
        {
            if (ModelState.IsValid)
            {
                db.AreaEspecialidad.Add(areaEspecialidad);
                await db.SaveChangesAsync();
                this.AddNotification("Área de Especialidad registrada exitosamente.", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }

            ViewBag.AreaGeneralId = new SelectList(db.AreaGeneral, "Id", "Descripcion", areaEspecialidad.AreaGeneralId);
            this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);
            return View(areaEspecialidad);
        }

        // GET: AreaEspecialidad/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Citas, Session))
            {
                this.AddNotification("No posees permisos para Editar Área de Especialidad.", NotificationType.WARNING);
                return RedirectToAction("Index", "AreaEspecialidad");
            }
            if (id == null)
            {
                return RedirectToAction("Error", "Home");;
            }
            AreaEspecialidad areaEspecialidad = await db.AreaEspecialidad.FindAsync(id);
            if (areaEspecialidad == null)
            {
                return HttpNotFound();
            }
            ViewBag.AreaGeneralId = new SelectList(db.AreaGeneral, "Id", "Descripcion", areaEspecialidad.AreaGeneralId);
            return View(areaEspecialidad);
        }

        // POST: AreaEspecialidad/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,AreaGeneralId,Descripcion,FechaCreacion,FechaModificacion,Estado,Eliminado")] AreaEspecialidad areaEspecialidad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(areaEspecialidad).State = EntityState.Modified;
                await db.SaveChangesAsync();
                this.AddNotification("Área de Especialidad modificada exitosamente.", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }
            ViewBag.AreaGeneralId = new SelectList(db.AreaGeneral, "Id", "Descripcion", areaEspecialidad.AreaGeneralId);
            this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);
            return View(areaEspecialidad);
        }

        // GET: AreaEspecialidad/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Citas, Session))
            {
                this.AddNotification("No posees permisos para Eliminar Área de Especialidad.", NotificationType.WARNING);
                return RedirectToAction("Index", "AreaEspecialidad");
            }
            if (id == null)
            {
                return RedirectToAction("Error", "Home");;
            }
            AreaEspecialidad areaEspecialidad = await db.AreaEspecialidad.FindAsync(id);
            if (areaEspecialidad == null)
            {
                return HttpNotFound();
            }
            return View(areaEspecialidad);
        }

        // POST: AreaEspecialidad/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AreaEspecialidad areaEspecialidad = await db.AreaEspecialidad.FindAsync(id);
            db.AreaEspecialidad.Remove(areaEspecialidad);
            await db.SaveChangesAsync();
            this.AddNotification("Área de Especialidad eliminada exitosamente.", NotificationType.SUCCESS);
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
