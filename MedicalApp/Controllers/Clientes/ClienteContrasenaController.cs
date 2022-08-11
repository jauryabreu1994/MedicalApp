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
    public class ClienteContrasenaController : Controller
    {
        private MainDbContext db = new MainDbContext();

        // GET: ClienteContrasena/ContraseñaCliente/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Clientes, Session))
            {
                this.AddNotification("No posees permisos para el Modificar esta Contraseña.", NotificationType.WARNING);
                return RedirectToAction("Index", "DashBoard");
            }

            if (id == null)
            {
                return RedirectToAction("Error", "Home");;
            }
            var clienteContrasena = await db.ClienteContrasena.FirstAsync(a => a.ClienteId == id && a.Eliminado == false);
            if (clienteContrasena == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClienteId = new SelectList(db.Cliente, "Id", "ClienteId", clienteContrasena.ClienteId);
            return View(clienteContrasena);
        }

        // POST: ClienteContrasena/ContraseñaCliente/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ClienteId,Contraseña,FechaCreacion,FechaModificacion,Estado,Eliminado")] ClienteContrasena clienteContrasena, 
                                             string ContrasenaActual, string RepetirContrasena)
        {
            if (ModelState.IsValid)
            {
                var contrasena = await db.ClienteContrasena.FirstAsync(a => a.ClienteId == clienteContrasena.ClienteId && a.Eliminado == false);
                string _contrasenaActual = new Encriptar_DesEncriptar().DesEncriptar(contrasena.Contraseña);
                if (ContrasenaActual != _contrasenaActual)
                    this.AddNotification("Contraseña actual NO coincide con la registrada.", NotificationType.ERROR);

                else if (clienteContrasena.Contraseña != RepetirContrasena)
                    this.AddNotification("Las Contraseñas NO coinciden.", NotificationType.ERROR);

                else if (clienteContrasena.Contraseña.Length < 6) 
                    this.AddNotification("La longitud de la Contraseña debe ser mayor a 6 caracteres.", NotificationType.ERROR);

                else if (clienteContrasena.Contraseña == ContrasenaActual)
                    this.AddNotification("Nueva Contraseña debe ser diferente a Contraseña Actual", NotificationType.ERROR);
                
                else
                {
                    contrasena.FechaModificacion = DateTime.Now;
                    contrasena.Contraseña = new Encriptar_DesEncriptar().Encriptar(clienteContrasena.Contraseña);

                    db.Entry(contrasena).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    this.AddNotification("Contraseña actualizada", NotificationType.SUCCESS);
                    return RedirectToAction("Index", "Cliente");
                }

                ViewBag.Contraseña = clienteContrasena.Contraseña;
                ViewBag.RepetirContrasena = RepetirContrasena;
                ViewBag.ContrasenaActual = ContrasenaActual;
            }

            ViewBag.ClienteId = new SelectList(db.Cliente, "Id", "ClienteId", clienteContrasena.ClienteId);
            return View(clienteContrasena);
        }

        public async Task<ActionResult> ResetPassword(int id)
        {
            ClienteContrasena contrasena = await db.ClienteContrasena.FirstAsync(a => a.Id == id && a.Eliminado == false);
            if (contrasena == null)
            {
                return HttpNotFound();
            }
            contrasena.FechaModificacion = DateTime.Now;
            contrasena.Contraseña = new Encriptar_DesEncriptar().Encriptar("123456");

            db.Entry(contrasena).State = EntityState.Modified;
            await db.SaveChangesAsync();

            this.AddNotification("Contraseña actualizada", NotificationType.SUCCESS);
            return RedirectToAction("Index", "Cliente");
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
