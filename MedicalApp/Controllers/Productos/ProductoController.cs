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
using MedicalApp.Models.Productos;
using MedicalApp.Extensions;
using MedicalApp.Controllers.Empresa;

namespace MedicalApp.Controllers.Productos
{
    public class ProductoController : Controller
    {
        private MainDbContext db = new MainDbContext();

        // GET: Producto
        public async Task<ActionResult> Index()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Servicios, Session))
            {
                this.AddNotification("No posees permisos para el Listado de Productos.", NotificationType.WARNING);
                return RedirectToAction("Index", "DashBoard");
            }
            var producto = db.Producto.Include(p => p._Empresa).Include(p => p._Impuesto);
            return View(await producto.OrderBy(a => a.Descripcion).ToListAsync());
        }

        // GET: Producto/Create
        public ActionResult Create()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Citas, Session))
            {
                this.AddNotification("No posees permisos para Crear Productos.", NotificationType.WARNING);
                return RedirectToAction("Index", "Producto");
            }
            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Nombre");
            ViewBag.ImpuestoId = new SelectList(db.Impuesto, "Id", "Descripcion");
            return View();
        }

        // POST: Producto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,EmpresaId,ProductoId,Descripcion,DescripcionExtendida,Costo,Venta,ImpuestoId,FechaCreacion,FechaModificacion,Estado,Eliminado")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(producto.ProductoId))
                    producto.ProductoId = new EmpresaController().GenerateNumber(true, Models.Enums.CompanyEnum.Servicio).Item1;

                producto.Venta = producto.Costo * ((db.Impuesto.Where(a => a.Id == producto.ImpuestoId).FirstOrDefault().Monto / 100) + 1);
                db.Producto.Add(producto);
                await db.SaveChangesAsync();
                this.AddNotification("Paciente registrado exitosamente.", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }

            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Nombre", producto.EmpresaId);
            ViewBag.ImpuestoId = new SelectList(db.Impuesto, "Id", "Descripcion", producto.ImpuestoId);
            this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);
            return View(producto);
        }

        // GET: Producto/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Citas, Session))
            {
                this.AddNotification("No posees permisos para Editar Productos.", NotificationType.WARNING);
                return RedirectToAction("Index", "Producto");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = await db.Producto.FindAsync(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Nombre", producto.EmpresaId);
            ViewBag.ImpuestoId = new SelectList(db.Impuesto, "Id", "Descripcion", producto.ImpuestoId);
            return View(producto);
        }

        // POST: Producto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,EmpresaId,ProductoId,Descripcion,DescripcionExtendida,Costo,Venta,ImpuestoId,FechaCreacion,FechaModificacion,Estado,Eliminado")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                producto.Venta = producto.Costo * ((db.Impuesto.Where(a => a.Id == producto.ImpuestoId).FirstOrDefault().Monto / 100) + 1);
                db.Entry(producto).State = EntityState.Modified;
                await db.SaveChangesAsync();
                this.AddNotification("Paciente modificado exitosamente.", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }
            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Nombre", producto.EmpresaId);
            ViewBag.ImpuestoId = new SelectList(db.Impuesto, "Id", "Descripcion", producto.ImpuestoId);
            this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);
            return View(producto);
        }

        // GET: Producto/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Citas, Session))
            {
                this.AddNotification("No posees permisos para Eliminar Productos.", NotificationType.WARNING);
                return RedirectToAction("Index", "Producto");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = await db.Producto.FindAsync(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Producto producto = await db.Producto.FindAsync(id);
            db.Producto.Remove(producto);
            await db.SaveChangesAsync();
            this.AddNotification("Producto eliminado exitosamente.", NotificationType.SUCCESS);
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
