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
    public class HabitacionController : Controller
    {
        private MainDbContext db = new MainDbContext();

        // GET: Habitacion
        public async Task<ActionResult> Index()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Habitaciones, Session))
            {
                this.AddNotification("No posees permisos para el Listado de Habitaciones.", NotificationType.WARNING);
                return RedirectToAction("Index", "DashBoard");
            }
            var habitacion = db.Habitacion.Include(h => h._EdificioNivel);
            return View(await habitacion.ToListAsync());
        }

        // GET: Habitacion/Create
        public ActionResult Create()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Habitaciones, Session))
            {
                this.AddNotification("No posees permisos para Crear Habitaciones.", NotificationType.WARNING);
                return RedirectToAction("Index", "Habitacion");
            }
            getEdificioNiveles();
            return View();
        }

        // POST: Habitacion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,EdificioNivelId,CodigoHabitacion,MaximoClientes,FechaCreacion,FechaModificacion,Estado,Eliminado")] Habitacion habitacion)
        {
            if (ModelState.IsValid)
            {
                db.Habitacion.Add(habitacion);
                await db.SaveChangesAsync();
                this.AddNotification("Habitacion registrado exitosamente.", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }

            getEdificioNiveles(habitacion.EdificioNivelId);
            this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);
            return View(habitacion);
        }

        // GET: Habitacion/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Habitaciones, Session))
            {
                this.AddNotification("No posees permisos para Editar Habitaciones.", NotificationType.WARNING);
                return RedirectToAction("Index", "Habitacion");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Habitacion habitacion = await db.Habitacion.FindAsync(id);
            if (habitacion == null)
            {
                return HttpNotFound();
            }
            getEdificioNiveles(habitacion.EdificioNivelId);
            return View(habitacion);
        }

        // POST: Habitacion/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,EdificioNivelId,CodigoHabitacion,MaximoClientes,FechaCreacion,FechaModificacion,Estado,Eliminado")] Habitacion habitacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(habitacion).State = EntityState.Modified;
                await db.SaveChangesAsync();
                this.AddNotification("Habitacion modificada exitosamente.", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }
            getEdificioNiveles(habitacion.EdificioNivelId); 
            this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);
            return View(habitacion);
        }

        // GET: Habitacion/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Habitaciones, Session))
            {
                this.AddNotification("No posees permisos para Eliminar Habitaciones.", NotificationType.WARNING);
                return RedirectToAction("Index", "Habitacion");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Habitacion habitacion = await db.Habitacion.FindAsync(id);
            if (habitacion == null)
            {
                return HttpNotFound();
            }
            return View(habitacion);
        }

        // POST: Habitacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Habitacion habitacion = await db.Habitacion.FindAsync(id);
            habitacion.FechaModificacion = DateTime.Now;
            habitacion.Eliminado = true;
            habitacion.Estado = Models.Enums.EstadoEnum.Inactivo;
            db.Entry(habitacion).State = EntityState.Modified;
            await db.SaveChangesAsync();
            this.AddNotification("Habitacion eliminada exitosamente", NotificationType.SUCCESS);
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


        private void getEdificioNiveles(int id = 1) 
        {
            var EdificioNiveles = db.EdificioNivel.Include(a => a._Edificio);
            foreach (var item in EdificioNiveles)
            {
                item.Descripcion = string.Format("{0}, Nivel: {1}", item._Edificio.Descripcion, item.Nivel);
            }

            ViewBag.EdificioNivelId = new SelectList(db.EdificioNivel.Include(a => a._Edificio), "Id", "Descripcion", id);
        }
            
    }
}
