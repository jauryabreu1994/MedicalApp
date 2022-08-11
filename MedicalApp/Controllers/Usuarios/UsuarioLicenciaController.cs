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
    public class UsuarioLicenciaController : Controller
    {
        private MainDbContext db = new MainDbContext();

        // GET: UsuarioLicencia
        public async Task<ActionResult> Index(int UsuarioId)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Usuario, Session))
            {
                this.AddNotification("No posees permisos para asignar licencia a los doctores", NotificationType.WARNING);
                return RedirectToAction("Index", "DashBoard");
            }
            var usuario = await db.Usuario.FindAsync(UsuarioId);
            if (usuario.GrupoUsuarioId != 4)
            {
                this.AddNotification("El Usuario seleccionado no es un doctor", NotificationType.WARNING);
                return RedirectToAction("Index", "Usuario");
            }

            var usuarioHorario = db.UsuarioLicencia.Where(a => a.UsuarioId == UsuarioId).Include(u => u._Usuario);
            ViewBag.UsuarioId = UsuarioId;
            return View(await usuarioHorario.ToListAsync());
        }

        // GET: UsuarioLicencia/Create
        public ActionResult Create(int UsuarioId)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Usuario, Session))
            {
                this.AddNotification("No posees permisos para crear licencia a los doctores", NotificationType.WARNING);
                return RedirectToAction("Index", "DashBoard");
            }
            var usuario = db.Usuario.Find(UsuarioId);
            if (usuario.GrupoUsuarioId != 4)
            {
                this.AddNotification("El Usuario seleccionado no es un doctor", NotificationType.WARNING);
                return RedirectToAction("Index", "Usuario");
            }

            ViewBag.UsuarioId = UsuarioId;
            ViewBag.Nombre = string.Format("{0} {1}", usuario.Nombre, usuario.Apellido);
            ViewBag.Fecha = DateTime.Now.Date.ToString("yyyy-MM-dd");
            return View();
        }

        // POST: UsuarioLicencia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,UsuarioId,FechaLicencia,FechaCreacion,FechaModificacion,Estado,Eliminado")] UsuarioLicencia usuarioLicencia)
        {
            if (ModelState.IsValid)
            {
                db.UsuarioLicencia.Add(usuarioLicencia);
                await db.SaveChangesAsync();
                this.AddNotification("Licencia registrada exitosamente", NotificationType.SUCCESS);
                return RedirectToAction("Index", "UsuarioLicencia", new { UsuarioId = usuarioLicencia.UsuarioId });
            }

            var usuario = db.Usuario.Find(usuarioLicencia.UsuarioId);

            this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);
            ViewBag.UsuarioId = usuario.Id;
            ViewBag.Nombre = string.Format("{0} {1}", usuario.Nombre, usuario.Apellido);
            ViewBag.Fecha = usuarioLicencia.FechaLicencia.Date.ToString("yyyy-MM-dd");
            return View(usuarioLicencia);
        }

        // GET: UsuarioLicencia/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Usuario, Session))
            {
                this.AddNotification("No posees permisos para eliminar licencia a los doctores", NotificationType.WARNING);
                return RedirectToAction("Index", "DashBoard");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuarioLicencia usuarioLicencia = await db.UsuarioLicencia.FindAsync(id);
            if (usuarioLicencia == null)
            {
                return HttpNotFound();
            }

            db.UsuarioLicencia.Remove(usuarioLicencia);
            await db.SaveChangesAsync();
            this.AddNotification("Licencia eliminada exitosamente", NotificationType.SUCCESS);
            return RedirectToAction("Index", "UsuarioLicencia", new { UsuarioId = usuarioLicencia.UsuarioId });
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
