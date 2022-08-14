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
using MedicalApp.Models.Empresas;
using MedicalApp.Models.Enums;
using MedicalApp.Models.Ubicaciones;
using MedicalApp.Extensions;

namespace MedicalApp.Controllers.Empresa
{
    public class EmpresaController : Controller
    {
        private MainDbContext db = new MainDbContext();

        // GET: Empresa
        public async Task<ActionResult> Index()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Empresa, Session))
            {
                this.AddNotification("No posees permisos para el Listado de Empresas", NotificationType.WARNING);
                return RedirectToAction("Index", "DashBoard");
            }
            var empresa = db.Empresa.Include(e => e._Ciudad).Include(e => e._Pais);
            return View(await empresa.ToListAsync());
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

        // GET: Empresa/Create
        public ActionResult Create()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Empresa, Session))
            {
                this.AddNotification("No posees permisos para Crear Empresas.", NotificationType.WARNING);
                return RedirectToAction("Index", "Empresa");
            }

            ViewBag.CiudadId = new SelectList(db.Ciudad, "Id", "Descripcion");
            ViewBag.PaisId = new SelectList(db.Pais, "Id", "CodigoArea");
            return View();
        }

        // POST: Empresa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nombre,Identificacion,NombreFiscal,PaisId,CiudadId,Direccion,Correo,Telefono,Logo,Moneda,CodiLicencia,ServicioId,ClienteId,CitaId,TransaccionId,CierreId,DoctorId,EnfermeraId,AdministrativoId,CorreoSMTP,ContrasenaSMTP,FechaCreacion,FechaModificacion,Estado,Eliminado")] Models.Empresas.Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                db.Empresa.Add(empresa);
                await db.SaveChangesAsync(); 
                this.AddNotification("Empresa registrada exitosamente.", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }

            ViewBag.CiudadId = new SelectList(db.Ciudad, "Id", "Descripcion", empresa.CiudadId);
            ViewBag.PaisId = new SelectList(db.Pais, "Id", "CodigoArea", empresa.PaisId);
            this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);
            return View(empresa);
        }

        // GET: Empresa/Edit/5
        public async Task<ActionResult> Edit()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Empresa, Session))
            {
                this.AddNotification("No posees permisos para Editar Empresas.", NotificationType.WARNING);
                return RedirectToAction("Index", "Empresa");
            }

            if (Session["EmpresaId"] == null )
            {
                return RedirectToAction("Error", "Home");
            }

            int id = Convert.ToInt32(Session["EmpresaId"].ToString());
            Models.Empresas.Empresa empresa = await db.Empresa.FindAsync(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }
            ViewBag.CiudadId = new SelectList(db.Ciudad, "Id", "Descripcion", empresa.CiudadId);
            ViewBag.PaisId = new SelectList(db.Pais, "Id", "CodigoArea", empresa.PaisId);
            return View(empresa);
        }

        // POST: Empresa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nombre,Identificacion,NombreFiscal,PaisId,CiudadId,Direccion,Correo,Telefono,Logo,Moneda,CodiLicencia,ServicioId,ClienteId,CitaId,TransaccionId,CierreId,DoctorId,EnfermeraId,AdministrativoId,CorreoSMTP,ContrasenaSMTP,FechaCreacion,FechaModificacion,Estado,Eliminado")] Models.Empresas.Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empresa).State = EntityState.Modified;
                await db.SaveChangesAsync();
                this.AddNotification("Empresa modificada exitosamente.", NotificationType.SUCCESS);
                return RedirectToAction("Edit");
            }
            ViewBag.CiudadId = new SelectList(db.Ciudad, "Id", "Descripcion", empresa.CiudadId);
            ViewBag.PaisId = new SelectList(db.Pais, "Id", "CodigoArea", empresa.PaisId); 
            this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);
            return View(empresa);
        }

        // GET: Empresa/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Empresa, Session))
            {
                this.AddNotification("No posees permisos para Eliminar Empresas.", NotificationType.WARNING);
                return RedirectToAction("Index", "Empresa");
            }
            if (id == null)
            {
                return RedirectToAction("Error", "Home");;
            }
            Models.Empresas.Empresa empresa = await db.Empresa.FindAsync(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }
            return View(empresa);
        }

        // POST: Empresa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Models.Empresas.Empresa empresa = await db.Empresa.FindAsync(id);
            db.Empresa.Remove(empresa);
            await db.SaveChangesAsync(); 
            this.AddNotification("Empresa eliminada exitosamente.", NotificationType.SUCCESS);
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

        protected internal Tuple<string, int> GenerateNumber(bool isSave = false, CompanyEnum companyType = CompanyEnum.Cita, int companyId = 0)
        {
            string valueToReturn = string.Empty;

            var emp = (companyId > 0) ? db.Empresa.Find(companyId) : db.Empresa.First();

            switch (companyType)
            {
                case CompanyEnum.Servicio:
                    valueToReturn = String.Format("S{0:000000000}", emp.ServicioId);
                    emp.ServicioId++;
                    break;
                case CompanyEnum.Cliente:
                    valueToReturn = String.Format("C{0:000000000}", emp.ClienteId);
                    emp.ClienteId++;
                    break;
                case CompanyEnum.Cita:
                    valueToReturn = String.Format("CI{0:000000000}", emp.CitaId);
                    emp.CitaId++;
                    break;
                case CompanyEnum.Transaccion:
                    valueToReturn = String.Format("T{0:000000000}", emp.TransaccionId);
                    emp.TransaccionId++;
                    break;
                case CompanyEnum.Cierre:
                    valueToReturn = String.Format("CL{0:000000000}", emp.CierreId);
                    emp.CierreId++;
                    break;
                case CompanyEnum.Doctor:
                    valueToReturn = String.Format("D{0:000000000}", emp.DoctorId);
                    emp.DoctorId++;
                    break;
                case CompanyEnum.Enfermera:
                    valueToReturn = String.Format("E{0:000000000}", emp.EnfermeraId);
                    emp.EnfermeraId++;
                    break;
                case CompanyEnum.Administrativo:
                    valueToReturn = String.Format("A{0:000000000}", emp.AdministrativoId);
                    emp.AdministrativoId++;
                    break;
            }

            if (isSave) {
                db.Entry(emp).State = EntityState.Modified;
                db.SaveChangesAsync();
            }
            return Tuple.Create(valueToReturn, emp.Id);
        }
    }
}
