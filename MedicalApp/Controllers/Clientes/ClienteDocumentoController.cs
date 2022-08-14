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
using System.IO;

namespace MedicalApp.Controllers.Clientes
{
    public class ClienteDocumentoController : Controller
    {
        private MainDbContext db = new MainDbContext();

        // GET: ClienteDocumento
        public async Task<ActionResult> Index(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Clientes, Session))
            {
                this.AddNotification("No posees permisos para el Listado de Documentos del Paciente.", NotificationType.WARNING);
                return RedirectToAction("Index", "DashBoard");
            }
            var clienteDocumento = db.ClienteDocumento.Where(a => a.ClienteHistorialId == id).Include(c => c._ClienteHistorial);
            ViewBag.Id = id;
            return View(await clienteDocumento.OrderBy(a => a.FechaCreacion).ToListAsync());
        }

        // GET: ClienteDocumento/Create
        public ActionResult Create(int Id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Clientes, Session))
            {
                this.AddNotification("No posees permisos para Crear Documentos al Paciente.", NotificationType.WARNING);
                return RedirectToAction("Index", "ClienteDocumento");
            }

            ViewBag.Id = Id;
            return View();
        }

        // POST: ClienteDocumento/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(int Id, string Referencia, HttpPostedFileBase file)
        {
            if (file != null) 
            {
                if (file.ContentLength > 0) {
                    string name = Path.GetFileName(file.FileName);
                    ClienteDocumento documento = new ClienteDocumento()
                    {
                        Id = 0,
                        Name = string.IsNullOrEmpty(Referencia) ? name : Referencia,
                        Extension = Path.GetExtension(name),
                        FechaCreacion = DateTime.Now,
                        FechaModificacion = DateTime.Now,
                        Estado = Models.Enums.EstadoEnum.Activo,
                        Eliminado = false,
                        ClienteHistorialId = Id,
                        Codigo = Guid.NewGuid(),
                    };
                    documento.RutaDocumento = Path.Combine(Server.MapPath("~/DocClientes"), documento.Codigo.ToString() + documento.Extension);

                    file.SaveAs(documento.RutaDocumento);
                    db.ClienteDocumento.Add(documento);
                    await db.SaveChangesAsync();
                    this.AddNotification("Documento de Paciente registrado exitosamente.", NotificationType.SUCCESS);
                    return RedirectToAction("Index", "ClienteDocumento", new { id = Id});
                }
            
            }

            ViewBag.Id = Id;
            this.AddNotification("Favor adjuntar el documento", NotificationType.ERROR);
            return View();
        }

        // GET: ClienteDocumento/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Clientes, Session))
            {
                this.AddNotification("No posees permisos para Eliminar Documentos al Paciente.", NotificationType.WARNING);
                return RedirectToAction("Index", "ClienteDocumento");
            }
            if (id == null)
            {
                return RedirectToAction("Error", "Home");;
            }
            ClienteDocumento clienteDocumento = await db.ClienteDocumento.FindAsync(id);
            if (clienteDocumento == null)
            {
                return HttpNotFound();
            }
            return View(clienteDocumento);
        }

        // POST: ClienteDocumento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ClienteDocumento clienteDocumento = await db.ClienteDocumento.FindAsync(id);
            db.ClienteDocumento.Remove(clienteDocumento);
            await db.SaveChangesAsync();
            this.AddNotification("Documento de Paciente eliminado exitosamente.", NotificationType.SUCCESS);
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
