using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using MedicalApp.Context;
using MedicalApp.Models.Clientes;
using MedicalApp.Models.Ubicaciones;
using MedicalApp.Controllers.Empresa;
using System;
using MedicalApp.Extensions;
using MedicalApp.Models.Enums;
using System.Text.RegularExpressions;

namespace MedicalApp.Controllers.Clientes
{
    public class ClienteController : Controller
    {
        private MainDbContext db = new MainDbContext();

        // GET: Cliente
        public async Task<ActionResult> Index()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Clientes, Session))
            {
                this.AddNotification("No posees permisos para el Listado de Clientes.", NotificationType.WARNING);
                return RedirectToAction("Index", "DashBoard");
            }
            var cliente = db.Cliente.Include(c => c._Ciudad).Include(c => c._Pais);
            return View(await cliente.OrderBy(a => a.Nombre).ToListAsync());
        }

        public JsonResult Ciudad_Bind(int paisId)
        {

            if (string.IsNullOrEmpty(paisId.ToString()))
            {
                return Json(null);
            }

            List<Ciudad> ciudades = db.Ciudad.Where(c => c.PaisId == paisId).ToList();
            List<SelectListItem> listado = new List<SelectListItem>();

            foreach (var ciudad in ciudades)
            {
                listado.Add(new SelectListItem { Text = ciudad.Descripcion, Value = ciudad.Id.ToString() });
            }

            return Json(listado, JsonRequestBehavior.AllowGet);
        }


        // GET: Cliente/Create
        public ActionResult Create()
        {

            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Clientes, Session))
            {
                this.AddNotification("No posees permisos para Crear Pacientes.", NotificationType.WARNING);
                return RedirectToAction("Index", "Cliente");
            }

            var data = new EmpresaController().GenerateNumber(false, Models.Enums.CompanyEnum.Cliente);
            ViewBag.ClienteId = data.Item1;
            ViewBag.CiudadId = new SelectList(db.Ciudad, "Id", "Descripcion");
            ViewBag.PaisId = new SelectList(db.Pais, "Id", "Descripcion");
            return View();
        }

        // POST: Cliente/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ClienteId,Nombre,Apellido,Identificacion,FechaNacimiento,NombreFiscal,PaisId,CiudadId,Direccion,Correo,Telefono,Genero,FechaCreacion,FechaModificacion,Estado,Eliminado")] Cliente cliente)
        {
            if (new GenericController().IdentificationExist(cliente.Identificacion, false)) 
            {

                ViewBag.CiudadId = new SelectList(db.Ciudad, "Id", "Descripcion", cliente.CiudadId);
                ViewBag.PaisId = new SelectList(db.Pais, "Id", "Descripcion", cliente.PaisId);
                this.AddNotification("Existe un cliente con esta cedula", NotificationType.ERROR);
                return View(cliente);
            }
            else if (ModelState.IsValid)
            {
                cliente.ClienteId = new EmpresaController().GenerateNumber(true, Models.Enums.CompanyEnum.Cliente).Item1;
                cliente.Identificacion = new GenericController().SetFormatVatNumber(cliente.Identificacion);
                cliente.Telefono = new GenericController().SetFormatPhoneNumer(cliente.Telefono);

                cliente.FechaModificacion = DateTime.Now;
                cliente.FechaCreacion = DateTime.Now;
                cliente.Direccion = string.IsNullOrEmpty(cliente.Direccion) ? string.Empty : cliente.Direccion;
                cliente.NombreFiscal = string.IsNullOrEmpty(cliente.NombreFiscal) ? string.Format("{0} {1}", cliente.Nombre, cliente.Apellido) : cliente.NombreFiscal;
                cliente = db.Cliente.Add(cliente);

                await db.SaveChangesAsync();
                Regex reg = new Regex("[*'\",_&#^@]");
                string Identificacion = reg.Replace(cliente.Identificacion, string.Empty);
                ClienteContrasena clienteContrasena = new ClienteContrasena()
                {
                    Id = 0,
                    ClienteId = cliente.Id,
                    Contraseña = new Encriptar_DesEncriptar().Encriptar(Identificacion),
                    FechaModificacion = DateTime.Now,
                    FechaCreacion = DateTime.Now,
                    Eliminado = false,
                    Estado = Models.Enums.EstadoEnum.Activo

                };
                db.ClienteContrasena.Add(clienteContrasena);
                await db.SaveChangesAsync();
                this.AddNotification("Paciente registrado exitosamente.", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }

            ViewBag.CiudadId = new SelectList(db.Ciudad, "Id", "Descripcion", cliente.CiudadId);
            ViewBag.PaisId = new SelectList(db.Pais, "Id", "Descripcion", cliente.PaisId);
            this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);
            return View(cliente);
        }

        // GET: Cliente/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Clientes, Session))
            {
                this.AddNotification("No posees permisos para Editar Pacientes.", NotificationType.WARNING);
                return RedirectToAction("Index", "Cliente");
            }

            if (id == null)
            {
                return RedirectToAction("Error", "Home");;
            }
            Cliente cliente = await db.Cliente.FindAsync(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            ViewBag.CiudadId = new SelectList(db.Ciudad, "Id", "Descripcion", cliente.CiudadId);
            ViewBag.PaisId = new SelectList(db.Pais, "Id", "Descripcion", cliente.PaisId);
            return View(cliente);
        }

        // POST: Cliente/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ClienteId,Nombre,Apellido,Identificacion,FechaNacimiento,NombreFiscal,PaisId,CiudadId,Direccion,Correo,Telefono,Genero,FechaCreacion,FechaModificacion,Estado,Eliminado")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {

                cliente.Identificacion = new GenericController().SetFormatVatNumber(cliente.Identificacion);
                cliente.Telefono = new GenericController().SetFormatPhoneNumer(cliente.Telefono);

                cliente.NombreFiscal = string.IsNullOrEmpty(cliente.NombreFiscal) ? string.Format("{0} {1}", cliente.Nombre, cliente.Apellido) : cliente.NombreFiscal;
                cliente.Direccion = string.IsNullOrEmpty(cliente.Direccion) ? string.Empty : cliente.Direccion;
                cliente.FechaModificacion = DateTime.Now;
                db.Entry(cliente).State = EntityState.Modified;
                await db.SaveChangesAsync();
                this.AddNotification("Paciente modificado exitosamente", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }
            ViewBag.CiudadId = new SelectList(db.Ciudad, "Id", "Descripcion", cliente.CiudadId);
            ViewBag.PaisId = new SelectList(db.Pais, "Id", "Descripcion", cliente.PaisId);
            this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);
            return View(cliente);
        }

        // GET: Cliente/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Clientes, Session))
            {
                this.AddNotification("No posees permisos para Eliminar Pacientes.", NotificationType.WARNING);
                return RedirectToAction("Index", "Cliente");
            }

            if (id == null)
            {
                return RedirectToAction("Error", "Home");;
            }
            Cliente cliente = await db.Cliente.FindAsync(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Cliente cliente = await db.Cliente.FindAsync(id);
            cliente.FechaModificacion = DateTime.Now;
            cliente.Eliminado = true;
            cliente.Estado = Models.Enums.EstadoEnum.Inactivo;
            db.Entry(cliente).State = EntityState.Modified;
            await db.SaveChangesAsync();
            this.AddNotification("Paciente eliminado exitosamente", NotificationType.SUCCESS);
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
