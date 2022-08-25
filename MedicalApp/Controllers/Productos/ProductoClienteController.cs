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

namespace MedicalApp.Controllers.Productos
{
    public class ProductoClienteController : Controller
    {
        private MainDbContext db = new MainDbContext();

        // GET: ProductoCliente
        public ActionResult Index(int id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Internamiento, Session))
            {
                this.AddNotification("No posees permisos para listar los productos del paciente internado", NotificationType.WARNING);
                return RedirectToAction("Index", "DashBoard");
            }
            if (id <= 0) 
            {
                this.AddNotification("Codigo de Habitacion de paciente no valido.", NotificationType.WARNING);
                return RedirectToAction("Index", "HabitacionCliente");
            }

            var producto = db.Producto.Include(p => p._Empresa).Include(p => p._Impuesto);

            var productoCliente = db.ProductoCliente.Where(a=>a.HabitacionClienteId == id).OrderBy(a => a.FechaCreacion).Include(p => p._HabitacionCliente).Include(p => p._Producto).ToList();
            var cliente = db.HabitacionCliente.Find(id)._Cliente;

            ViewBag.HabitacionClienteId = id;
            ViewBag.Cliente = string.Format("{0} {1}", cliente.Nombre, cliente.Apellido);

            return View(productoCliente);
        }

        // GET: ProductoCliente/Create
        public ActionResult Create(int id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Internamiento, Session))
            {
                this.AddNotification("No posees permisos para el agregarle medicamenteos al paciente interno", NotificationType.WARNING);
                return RedirectToAction("Index", "DashBoard");
            }
            var producto = db.Producto.Include(p => p._Empresa).Include(p => p._Impuesto);
            ViewBag.HabitacionClienteId = id;

            var habitacionCliente = db.HabitacionCliente.Where(a=>a.Id == id).Include(h => h._Cliente).Include(h => h._Habitacion)
                                                                                    .Include(h => h._Doctor).Include(h => h._Cajero)
                                                                                    .Include(h => h._Enfermera).First();

            ViewBag.HabitacionCliente = habitacionCliente._Habitacion.CodigoHabitacion;
            ViewBag.Paciente = string.Format("{0} {1}", habitacionCliente._Cliente.Nombre, habitacionCliente._Cliente.Apellido);
            ViewBag.Doctor = string.Format("{0} {1}", habitacionCliente._Doctor.Nombre, habitacionCliente._Doctor.Apellido);
            ViewBag.Enfermera = string.Format("{0} {1}", habitacionCliente._Enfermera.Nombre, habitacionCliente._Enfermera.Apellido);
            ViewBag.Productos = db.Producto.Where(a => !a.Eliminado).ToList();

            ViewBag.ProductoId = 0;
            ViewBag.Producto = string.Empty;
            return View();
        }

        // POST: ProductoCliente/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,HabitacionClienteId,ProductoId,Cantidad,Descripcion,FechaCreacion,FechaModificacion,Estado,Eliminado")] ProductoCliente productoCliente)
        {
            if (ModelState.IsValid)
            {
                db.ProductoCliente.Add(productoCliente);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "ProductoCliente",new { id = productoCliente.HabitacionClienteId });
            }

            var producto = db.Producto.Include(p => p._Empresa).Include(p => p._Impuesto);
            ViewBag.HabitacionClienteId = productoCliente.HabitacionClienteId;
            ViewBag.Productos = db.Producto.Where(a => !a.Eliminado).ToList();
            ViewBag.ProductoId = productoCliente.ProductoId;
            ViewBag.Producto = producto.Where(a => a.Id == productoCliente.ProductoId).FirstOrDefault().Descripcion;
            return View(productoCliente);
        }

        // GET: ProductoCliente/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductoCliente productoCliente = await db.ProductoCliente.FindAsync(id);
            if (productoCliente == null)
            {
                return HttpNotFound();
            }
            ViewBag.HabitacionClienteId = new SelectList(db.HabitacionCliente, "Id", "Id", productoCliente.HabitacionClienteId);
            ViewBag.ProductoId = new SelectList(db.Producto, "Id", "ProductoId", productoCliente.ProductoId);
            return View(productoCliente);
        }

        // POST: ProductoCliente/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,HabitacionClienteId,ProductoId,Cantidad,Descripcion,FechaCreacion,FechaModificacion,Estado,Eliminado")] ProductoCliente productoCliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productoCliente).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.HabitacionClienteId = new SelectList(db.HabitacionCliente, "Id", "Id", productoCliente.HabitacionClienteId);
            ViewBag.ProductoId = new SelectList(db.Producto, "Id", "ProductoId", productoCliente.ProductoId);
            return View(productoCliente);
        }

        public async Task<ActionResult> Apply(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Internamiento, Session))
            {
                this.AddNotification("No posees permisos para aplicar medicamentos al pacientes", NotificationType.WARNING);
                return RedirectToAction("Index", "HabitacionCliente");
            }


            ProductoCliente productoCliente = await db.ProductoCliente.FindAsync(id);
            productoCliente.FechaModificacion = DateTime.Now;
            productoCliente.Estado = Models.Enums.EstadoEnum.Completa;
            int HabitacionClienteId = productoCliente.HabitacionClienteId;
            db.Entry(productoCliente).State = EntityState.Modified;
            await db.SaveChangesAsync();
            this.AddNotification("Medicamento Aplicado", NotificationType.SUCCESS);
            return RedirectToAction("Index", "ProductoCliente", new { id = HabitacionClienteId });
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Internamiento, Session))
            {
                this.AddNotification("No posees permisos para aplicar medicamentos al pacientes", NotificationType.WARNING);
                return RedirectToAction("Index", "HabitacionCliente");
            }


            ProductoCliente productoCliente = await db.ProductoCliente.FindAsync(id);
            productoCliente.FechaModificacion = DateTime.Now;
            productoCliente.Estado = Models.Enums.EstadoEnum.Inactivo;
            productoCliente.Eliminado = true;
            int HabitacionClienteId = productoCliente.HabitacionClienteId;
            db.Entry(productoCliente).State = EntityState.Modified;
            await db.SaveChangesAsync();
            this.AddNotification("Medicamento Aplicado", NotificationType.SUCCESS);
            return RedirectToAction("Index", "ProductoCliente", new { id = HabitacionClienteId });
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
