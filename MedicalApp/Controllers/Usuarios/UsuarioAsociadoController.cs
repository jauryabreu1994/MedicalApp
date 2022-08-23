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
    public class UsuarioAsociadoController : Controller
    {
        private MainDbContext db = new MainDbContext();

        // GET: UsuarioAsociado
        public async Task<ActionResult> Index(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Usuario, Session))
            {
                this.AddNotification("No posees permisos para el Listado de Usuarios Asociados.", NotificationType.WARNING);
                return RedirectToAction("Index", "DashBoard");
            }
            if (id != null)
            {
                ViewBag.UsuarioId = id;
                var usuarioAsociado = db.UsuarioAsociado.Include(u => u._Asistente).Include(u => u._Doctor).Where(a=>a.DoctorId == id && a.Estado == Models.Enums.EstadoEnum.Activo);
                return View(await usuarioAsociado.OrderBy(a => a.AsistenteId).ToListAsync());
            }
            return RedirectToAction("Index");
        }

        // GET: UsuarioAsociado/Create
        public ActionResult Create(int id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Usuario, Session))
            {
                this.AddNotification("No posees permisos para Crear Usuarios Asociados.", NotificationType.WARNING);
                return RedirectToAction("Index", "UsuarioAsociado");
            }
            var list = getList();
            ViewBag.AsistenteId = new SelectList(list.Where(a=>a.Id != id && a.GrupoUsuarioId == 5).ToList(), "Id", "UsuarioId");
            ViewBag.DoctorId = new SelectList(list.Where(a => a.Id == id).ToList(), "Id", "UsuarioId", id);
            ViewBag.Id = id;
            return View();
        }

        // POST: UsuarioAsociado/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,DoctorId,AsistenteId,FechaCreacion,FechaModificacion,Estado,Eliminado")] UsuarioAsociado usuarioAsociado)
        {
            if (ModelState.IsValid)
            {
                db.UsuarioAsociado.Add(usuarioAsociado);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { id = usuarioAsociado.DoctorId });
            }

            var list = getList();
            ViewBag.AsistenteId = new SelectList(list.Where(a => a.Id != usuarioAsociado.DoctorId && a.GrupoUsuarioId == 5).ToList(), "Id", "UsuarioId", usuarioAsociado.AsistenteId);
            ViewBag.DoctorId = new SelectList(list.Where(a => a.Id == usuarioAsociado.DoctorId).ToList(), "Id", "UsuarioId", usuarioAsociado.DoctorId);
            ViewBag.Id = usuarioAsociado.DoctorId;
            return View(usuarioAsociado);
        }

        // GET: UsuarioAsociado/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Usuario, Session))
            {
                this.AddNotification("No posees permisos para Editar Usuarios Asociados.", NotificationType.WARNING);
                return RedirectToAction("Index", "UsuarioAsociado");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuarioAsociado usuarioAsociado = await db.UsuarioAsociado.FindAsync(id);
            if (usuarioAsociado == null)
            {
                return HttpNotFound();
            }
            var list = getList();
            ViewBag.AsistenteId = new SelectList(list.Where(a => a.Id != usuarioAsociado.DoctorId && a.GrupoUsuarioId == 5).ToList(), "Id", "UsuarioId", usuarioAsociado.AsistenteId);
            ViewBag.DoctorId = new SelectList(list.Where(a => a.Id == usuarioAsociado.DoctorId).ToList(), "Id", "UsuarioId", usuarioAsociado.DoctorId);
            ViewBag.Id = usuarioAsociado.DoctorId;
            return View(usuarioAsociado);
        }

        // POST: UsuarioAsociado/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,DoctorId,AsistenteId,FechaCreacion,FechaModificacion,Estado,Eliminado")] UsuarioAsociado usuarioAsociado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuarioAsociado).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { id = usuarioAsociado.DoctorId });
            }
            var list = getList();
            ViewBag.AsistenteId = new SelectList(list.Where(a => a.Id != usuarioAsociado.DoctorId && a.GrupoUsuarioId == 5).ToList(), "Id", "UsuarioId", usuarioAsociado.AsistenteId);
            ViewBag.DoctorId = new SelectList(list.Where(a => a.Id == usuarioAsociado.DoctorId).ToList(), "Id", "UsuarioId", usuarioAsociado.DoctorId);
            ViewBag.Id = usuarioAsociado.DoctorId;
            this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);
            return View(usuarioAsociado);
        }

        // GET: UsuarioAsociado/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Usuario, Session))
            {
                this.AddNotification("No posees permisos para Eliminar Usuarios Asociados.", NotificationType.WARNING);
                return RedirectToAction("Index", "UsuarioAsociado");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuarioAsociado usuarioAsociado = await db.UsuarioAsociado.FindAsync(id);
            if (usuarioAsociado == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = usuarioAsociado.DoctorId;
            return View(usuarioAsociado);
        }

        // POST: UsuarioAsociado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            UsuarioAsociado usuarioAsociado = await db.UsuarioAsociado.FindAsync(id);
            usuarioAsociado.FechaModificacion = DateTime.Now;
            usuarioAsociado.Eliminado = true;
            usuarioAsociado.Estado = Models.Enums.EstadoEnum.Inactivo;
            db.Entry(usuarioAsociado).State = EntityState.Modified;
            await db.SaveChangesAsync();
            this.AddNotification("Asistente desvinculado del Doctor exitosamente", NotificationType.SUCCESS);
            return RedirectToAction("Index", new { id = usuarioAsociado.DoctorId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private List<Usuario> getList() {

            List<Usuario> list = db.Usuario.ToList();

            foreach (var item in list)
                item.UsuarioId = string.Format("{0} {1}", item.Nombre, item.Apellido);

            return list;
        }
    }
}
