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
using MedicalApp.Models.Clientes;
using MedicalApp.Extensions;

namespace MedicalApp.Controllers.Clientes
{
    public class ClienteHistorialController : Controller
    {
        private MainDbContext db = new MainDbContext();

        // GET: ClienteHistorial
        public async Task<ActionResult> Index(int Id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Clientes, Session))
            {
                this.AddNotification("No posees permisos para el Historial del Paciente.", NotificationType.WARNING);
                return RedirectToAction("Index", "DashBoard");
            }
            var clienteHistorial = db.ClienteHistorial.Where(a=>a.ClienteId == Id).Include(c => c._Cliente).Include(c => c._Usuario);

            ViewBag.Id = Id;
            return View(await clienteHistorial.ToListAsync());
        }

        // GET: ClienteHistorial/Create
        public ActionResult Create(int id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Clientes, Session))
            {
                this.AddNotification("No posees permisos para crear un  Historial de Paciente.", NotificationType.WARNING);
                return RedirectToAction("Index", "ClienteHistorial");
            }
            ViewBag.ClienteId = new SelectList(db.Cliente, "Id", "ClienteId");
            ViewBag.UsuarioId = new SelectList(db.Usuario, "Id", "UsuarioId");
            ViewBag.Id = id;
            return View();
        }

        // POST: ClienteHistorial/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ClienteId,UsuarioId,Documentacion,Indicaciones,FechaCreacion,FechaModificacion,Estado,Eliminado")] ClienteHistorial clienteHistorial)
        {
            if (ModelState.IsValid)
            {
                int UserID = Convert.ToInt32(new Encriptar_DesEncriptar().DesEncriptar(Session["UserID"].ToString()));
                clienteHistorial.UsuarioId = UserID;
                db.ClienteHistorial.Add(clienteHistorial);
                await db.SaveChangesAsync();
                this.AddNotification("Historial de Paciente registrado exitosamente.", NotificationType.SUCCESS);
                return RedirectToAction("Index", "ClienteHistorial", new { id = clienteHistorial .ClienteId});
            }

            ViewBag.ClienteId = new SelectList(db.Cliente, "Id", "ClienteId", clienteHistorial.ClienteId);
            ViewBag.UsuarioId = new SelectList(db.Usuario, "Id", "UsuarioId", clienteHistorial.UsuarioId);
            this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);
            return View(clienteHistorial);
        }

        // GET: ClienteHistorial/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Clientes, Session))
            {
                this.AddNotification("No posees permisos para eliminar un  Historial de Paciente.", NotificationType.WARNING);
                return RedirectToAction("Index", "ClienteHistorial");
            }
            if (id == null)
            {
                return RedirectToAction("Error", "Home");;
            }
            ClienteHistorial clienteHistorial = await db.ClienteHistorial.FindAsync(id);
            if (clienteHistorial == null)
            {
                return HttpNotFound();
            }
            return View(clienteHistorial);
        }

        // POST: ClienteHistorial/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ClienteHistorial clienteHistorial = await db.ClienteHistorial.FindAsync(id);
            db.ClienteHistorial.Remove(clienteHistorial);
            await db.SaveChangesAsync();
            this.AddNotification("Historial de Paciente eliminado exitosamente.", NotificationType.SUCCESS);
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
