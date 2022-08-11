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
using MedicalApp.Models.Horarios;

namespace MedicalApp.Controllers.Usuarios
{
    public class UsuarioHorarioController : Controller
    {
        private MainDbContext db = new MainDbContext();

        // GET: UsuarioHorario
        public async Task<ActionResult> Index(int UsuarioId)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Usuario, Session))
            {
                this.AddNotification("No posees permisos para asignar Horario a los doctores", NotificationType.WARNING);
                return RedirectToAction("Index", "DashBoard");
            }
            var usuario = await db.Usuario.FindAsync(UsuarioId);
            if (usuario.GrupoUsuarioId != 4)
            {
                this.AddNotification("El Usuario seleccionado no es un doctor", NotificationType.WARNING);
                return RedirectToAction("Index", "Usuario");
            }

            var usuarioHorario = db.UsuarioHorario.Where(a=> a.UsuarioId == UsuarioId).Include(u => u._Doctor).Include(u => u._Horario);
            ViewBag.UsuarioId = UsuarioId;
            return View(await usuarioHorario.ToListAsync());
        }


        public JsonResult Horario_Bind(int dia, int userId)
        {

            if (string.IsNullOrEmpty(dia.ToString()) || string.IsNullOrEmpty(userId.ToString()))
            {
                return Json(null);
            }

            List<Horario> horarios = db.Horario.ToList();
            List<UsuarioHorario> usuariohorarios = db.UsuarioHorario.Where(a=>a.UsuarioId == userId && a.Dia == (Models.Enums.DiasEnum)dia).ToList();
            List<SelectListItem> listado = new List<SelectListItem>();

            if (usuariohorarios.Count > 0)
            {
                foreach (var h in horarios)
                {
                    if (!usuariohorarios.Exists(a => a.HorarioId == h.Id))
                        listado.Add(new SelectListItem { Text = string.Format("{0}:{1}", h.Hora.ToString("00"), h.Minutos.ToString("00")), Value = h.Id.ToString() });
                }
            }
            else
            {
                foreach (var h in horarios)
                    listado.Add(new SelectListItem { Text = string.Format("{0}:{1}", h.Hora.ToString("00"), h.Minutos.ToString("00")), Value = h.Id.ToString() });
            }

            return Json(listado, JsonRequestBehavior.AllowGet);
        }


        // GET: UsuarioHorario/Create
        public ActionResult Create(int UsuarioId)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Usuario, Session))
            {
                this.AddNotification("No posees permisos para asignar Horario a los doctores", NotificationType.WARNING);
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
            return View();
        }

        // POST: UsuarioHorario/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,UsuarioId,Dia,HorarioId,CantidadPacientes,FechaCreacion,FechaModificacion,Estado,Eliminado")] UsuarioHorario usuarioHorario)
        {
            if (ModelState.IsValid)
            {
                if (db.UsuarioHorario.Where(a => a.UsuarioId == usuarioHorario.UsuarioId && a.Dia == usuarioHorario.Dia &&
                                                a.HorarioId == usuarioHorario.HorarioId).Count() == 0)
                {
                    db.UsuarioHorario.Add(usuarioHorario);
                    await db.SaveChangesAsync();

                    this.AddNotification("Horario registrado exitosamente", NotificationType.SUCCESS);
                    return RedirectToAction("Index", "UsuarioHorario", new { UsuarioId = usuarioHorario.UsuarioId });
                }else

                    this.AddNotification("Horario registrado", NotificationType.ERROR);
            }else
                this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);

            var usuario = db.Usuario.Find(usuarioHorario.UsuarioId);
            ViewBag.UsuarioId = usuario.Id;
            ViewBag.Nombre = string.Format("{0} {1}", usuario.Nombre, usuario.Apellido);
            return View(usuarioHorario);
        }

        // GET: UsuarioHorario/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Usuario, Session))
            {
                this.AddNotification("No posees permisos para modificar Horario a los doctores", NotificationType.WARNING);
                return RedirectToAction("Index", "DashBoard");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuarioHorario usuarioHorario = await db.UsuarioHorario.FindAsync(id);
            if (usuarioHorario == null)
            {
                return HttpNotFound();
            }
            var usuario = db.Usuario.Find(usuarioHorario.UsuarioId);
            ViewBag.UsuarioId = usuario.Id;
            ViewBag.Nombre = string.Format("{0} {1}", usuario.Nombre, usuario.Apellido);

            ViewBag.Dia = usuarioHorario.Dia.ToString();
            ViewBag.Hora = string.Format("{0}:{1}", usuarioHorario._Horario.Hora.ToString("00"), usuarioHorario._Horario.Minutos.ToString("00"));
            return View(usuarioHorario);
        }

        // POST: UsuarioHorario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,UsuarioId,Dia,HorarioId,CantidadPacientes,FechaCreacion,FechaModificacion,Estado,Eliminado")] UsuarioHorario usuarioHorario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuarioHorario).State = EntityState.Modified;
                await db.SaveChangesAsync();
                this.AddNotification("Cantidad de pacientes disponible por horario modificado exitosamente", NotificationType.SUCCESS);
                return RedirectToAction("Index", "UsuarioHorario", new { UsuarioId = usuarioHorario.UsuarioId });
            }

            var usuario = db.Usuario.Find(usuarioHorario.UsuarioId);
            ViewBag.UsuarioId = usuario.UsuarioId;
            ViewBag.Nombre = string.Format("{0} {1}", usuario.Nombre, usuario.Apellido);
            usuarioHorario = await db.UsuarioHorario.FindAsync(usuarioHorario.Id);
            ViewBag.Dia = usuarioHorario.Dia.ToString();
            ViewBag.Hora = string.Format("{0}:{1}", usuarioHorario._Horario.Hora.ToString("00"), usuarioHorario._Horario.Minutos.ToString("00"));

            return View(usuarioHorario);
        }

        // GET: UsuarioHorario/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Usuario, Session))
            {
                this.AddNotification("No posees permisos para eliminar Horario a los doctores", NotificationType.WARNING);
                return RedirectToAction("Index", "DashBoard");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuarioHorario usuarioHorario = await db.UsuarioHorario.FindAsync(id);
            if (usuarioHorario == null)
            {
                return HttpNotFound();
            }

            db.UsuarioHorario.Remove(usuarioHorario);
            await db.SaveChangesAsync();
            this.AddNotification("Horario eliminado exitosamente", NotificationType.SUCCESS);
            return RedirectToAction("Index", "UsuarioHorario", new { UsuarioId = usuarioHorario.UsuarioId });
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
