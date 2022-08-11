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
    public class AreaGeneralController : Controller
    {
        private MainDbContext db = new MainDbContext();

        // GET: AreaGeneral
        public async Task<ActionResult> Index()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Usuario, Session))
            {
                this.AddNotification("No posees permisos para el Listado de Área General.", NotificationType.WARNING);
                return RedirectToAction("Index", "DashBoard");
            }
            return View(await db.AreaGeneral.ToListAsync());
        }

        // GET: AreaGeneral/Create
        public ActionResult Create()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Usuario, Session))
            {
                this.AddNotification("No posees permisos para Crear Área General.", NotificationType.WARNING);
                return RedirectToAction("Index", "AreaGeneral");
            }
            return View();
        }

        // POST: AreaGeneral/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Descripcion,FechaCreacion,FechaModificacion,Estado,Eliminado")] AreaGeneral areaGeneral)
        {
            if (ModelState.IsValid)
            {
                db.AreaGeneral.Add(areaGeneral);
                await db.SaveChangesAsync();
                this.AddNotification("Área General registrada exitosamente.", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }
            this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);
            return View(areaGeneral);
        }

        // GET: AreaGeneral/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Usuario, Session))
            {
                this.AddNotification("No posees permisos para Editar Área General.", NotificationType.WARNING);
                return RedirectToAction("Index", "AreaGeneral");
            }
            if (id == null)
            {
                return RedirectToAction("Error", "Home");;
            }
            AreaGeneral areaGeneral = await db.AreaGeneral.FindAsync(id);
            if (areaGeneral == null)
            {
                return HttpNotFound();
            }
            return View(areaGeneral);
        }

        // POST: AreaGeneral/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Descripcion,FechaCreacion,FechaModificacion,Estado,Eliminado")] AreaGeneral areaGeneral)
        {
            if (ModelState.IsValid)
            {
                db.Entry(areaGeneral).State = EntityState.Modified;
                await db.SaveChangesAsync();
                this.AddNotification("Área General modificada exitosamente.", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }
            this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);
            return View(areaGeneral);
        }

        // GET: AreaGeneral/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Usuario, Session))
            {
                this.AddNotification("No posees permisos para Eliminar Área General.", NotificationType.WARNING);
                return RedirectToAction("Index", "AreaGeneral");
            }
            if (id == null)
            {
                return RedirectToAction("Error", "Home");;
            }
            AreaGeneral areaGeneral = await db.AreaGeneral.FindAsync(id);
            if (areaGeneral == null)
            {
                return HttpNotFound();
            }
            return View(areaGeneral);
        }

        // POST: AreaGeneral/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AreaGeneral areaGeneral = await db.AreaGeneral.FindAsync(id);
            db.AreaGeneral.Remove(areaGeneral);
            await db.SaveChangesAsync();
            this.AddNotification("Área General eliminada exitosamente.", NotificationType.SUCCESS);
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
