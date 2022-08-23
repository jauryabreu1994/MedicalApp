using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using MedicalApp.Context;
using MedicalApp.Models.Servicios;
using MedicalApp.Extensions;
using System;
using System.Linq;
using MedicalApp.Controllers.Empresa;

namespace MedicalApp.Controllers.Servicios
{
    public class ServicioController : Controller
    {
        private MainDbContext db = new MainDbContext();

        // GET: Servicio
        public async Task<ActionResult> Index()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Servicios, Session))
            {
                this.AddNotification("No posees permisos para el Listado de Servicio.", NotificationType.WARNING);
                return RedirectToAction("Index", "DashBoard");
            }
            var servicio = db.Servicio.Include(s => s._Empresa).Include(s => s._Impuesto).Include(s => s._Usuario);
            return View(await servicio.OrderBy(a => a.Descripcion).ToListAsync());
        }

        // GET: Servicio/Create
        public ActionResult Create()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Servicios, Session))
            {
                this.AddNotification("No posees permisos para Crear Servicios.", NotificationType.WARNING);
                return RedirectToAction("Index", "Servicio");
            }
            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Nombre");
            ViewBag.ImpuestoId = new SelectList(db.Impuesto, "Id", "Descripcion");
            ViewBag.UsuarioId = new SelectList(db.Usuario, "Id", "UsuarioId");
            ViewBag.Usuarios = db.Usuario.Include(u => u._AreaEspecialidad).Include(u => u._Ciudad).Include(u => u._Empresa).Include(u => u._GrupoUsuario).Include(u => u._Pais).ToList();
            return View();
        }

        // POST: Servicio/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,EmpresaId,ServicioId,Descripcion,DescripcionExtendida,UsuarioId,Costo,ImpuestoId,FechaCreacion,FechaModificacion,Estado,Eliminado")] Servicio servicio)
        {
            if (ModelState.IsValid)
            {

                if (string.IsNullOrEmpty(servicio.ServicioId))
                    servicio.ServicioId = new EmpresaController().GenerateNumber(true, Models.Enums.CompanyEnum.Servicio).Item1;

                db.Servicio.Add(servicio);
                await db.SaveChangesAsync();
                this.AddNotification("Servicio registrado exitosamente.", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }

            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Nombre", servicio.EmpresaId);
            ViewBag.ImpuestoId = new SelectList(db.Impuesto, "Id", "Descripcion", servicio.ImpuestoId);
            ViewBag.UsuarioId = new SelectList(db.Usuario, "Id", "UsuarioId", servicio.UsuarioId);
            ViewBag.Usuarios = db.Usuario.Include(u => u._AreaEspecialidad).Include(u => u._Ciudad).Include(u => u._Empresa).Include(u => u._GrupoUsuario).Include(u => u._Pais).ToList();
            this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);
            return View(servicio);
        }

        // GET: Servicio/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Servicios, Session))
            {
                this.AddNotification("No posees permisos para Editar Servicios.", NotificationType.WARNING);
                return RedirectToAction("Index", "Servicio");
            }
            if (id == null)
            {
                return RedirectToAction("Error", "Home");;
            }
            Servicio servicio = await db.Servicio.FindAsync(id);
            if (servicio == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Nombre", servicio.EmpresaId);
            ViewBag.ImpuestoId = new SelectList(db.Impuesto, "Id", "Descripcion", servicio.ImpuestoId);
            ViewBag.UsuarioId = new SelectList(db.Usuario, "Id", "UsuarioId", servicio.UsuarioId);
            ViewBag.Usuarios = db.Usuario.Include(u => u._AreaEspecialidad).Include(u => u._Ciudad).Include(u => u._Empresa).Include(u => u._GrupoUsuario).Include(u => u._Pais).ToList();

            ViewBag.UserId = servicio.UsuarioId;
            ViewBag.UserName = string.Format("{0} {1}", servicio._Usuario.Nombre, servicio._Usuario.Apellido);
            return View(servicio);
        }

        // POST: Servicio/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,EmpresaId,ServicioId,Descripcion,DescripcionExtendida,UsuarioId,Costo,ImpuestoId,FechaCreacion,FechaModificacion,Estado,Eliminado")] Servicio servicio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(servicio).State = EntityState.Modified;
                await db.SaveChangesAsync();
                this.AddNotification("Servicio modificado exitosamente.", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }
            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Nombre", servicio.EmpresaId);
            ViewBag.ImpuestoId = new SelectList(db.Impuesto, "Id", "Descripcion", servicio.ImpuestoId);
            ViewBag.UsuarioId = new SelectList(db.Usuario, "Id", "UsuarioId", servicio.UsuarioId);
            ViewBag.Usuarios = db.Usuario.Include(u => u._AreaEspecialidad).Include(u => u._Ciudad).Include(u => u._Empresa).Include(u => u._GrupoUsuario).Include(u => u._Pais).ToList();
            this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);
            return View(servicio);
        }

        // GET: Servicio/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Servicios, Session))
            {
                this.AddNotification("No posees permisos para Eliminar Servicios.", NotificationType.WARNING);
                return RedirectToAction("Index", "Servicio");
            }
            if (id == null)
            {
                return RedirectToAction("Error", "Home");;
            }
            Servicio servicio = await db.Servicio.FindAsync(id);
            if (servicio == null)
            {
                return HttpNotFound();
            }
            return View(servicio);
        }

        // POST: Servicio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Servicio servicio = await db.Servicio.FindAsync(id);
            servicio.FechaModificacion = DateTime.Now;
            servicio.Eliminado = true;
            servicio.Estado = Models.Enums.EstadoEnum.Inactivo;
            db.Entry(servicio).State = EntityState.Modified;
            await db.SaveChangesAsync();
            this.AddNotification("Servicio eliminado exitosamente", NotificationType.SUCCESS);
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
