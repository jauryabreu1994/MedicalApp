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
using MedicalApp.Models.Edificios;
using MedicalApp.Extensions;

namespace MedicalApp.Controllers.Edificios
{
    public class HabitacionClienteController : Controller
    {
        private MainDbContext db = new MainDbContext();

        // GET: HabitacionCliente
        public async Task<ActionResult> Index()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Internamiento, Session))
            {
                this.AddNotification("No posees permisos para el Listado de Habitaciones por Clientes.", NotificationType.WARNING);
                return RedirectToAction("Index", "DashBoard");
            }
            var habitacionCliente = db.HabitacionCliente.Include(h => h._Cliente).Include(h => h._Habitacion)
                                                        .Include(h => h._Doctor).Include(h => h._Cajero)
                                                        .Include(h => h._Enfermera);
            return View(await habitacionCliente.ToListAsync());
        }

        // GET: HabitacionCliente/Create
        public ActionResult Create()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Internamiento, Session))
            {
                this.AddNotification("No posees permisos para Crear Habitaciones por Clientes.", NotificationType.WARNING);
                return RedirectToAction("Index", "HabitacionCliente");
            }
            getListData();
            return View();
        }

        // POST: HabitacionCliente/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(int HabitacionId, string Habitacion,
                                int ClienteId, string Cliente,
                                int DoctorId, string Doctor,
                                int EnfermeraId, string Enfermera,
                                int CajeroId, string Cajero)
        {
            if (!string.IsNullOrEmpty(Habitacion) && !string.IsNullOrEmpty(Cliente) &&
                 !string.IsNullOrEmpty(Doctor) && !string.IsNullOrEmpty(Enfermera) &&
                 !string.IsNullOrEmpty(Cajero))
            {
                HabitacionCliente habitacionCliente = new HabitacionCliente()
                {
                    Id = 0,
                    CajeroId = CajeroId,
                    ClienteId = ClienteId,
                    DoctorId = DoctorId,
                    EnfermeraId = EnfermeraId,
                    HabitacionId = HabitacionId,
                    FechaCreacion = DateTime.Now,
                    FechaModificacion = DateTime.Now,
                    Estado = Models.Enums.EstadoEnum.Activo,
                    Eliminado = false
                };

                db.HabitacionCliente.Add(habitacionCliente);
                await db.SaveChangesAsync();
                this.AddNotification("Asignacion de Habitacion al Paciente registrado exitosamente.", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }
            else
            {

                this.AddNotification("Es necesario completar todas las opciones", NotificationType.ERROR);
                getListData(HabitacionId, Habitacion, ClienteId, Cliente,
                            DoctorId, Doctor, EnfermeraId, Enfermera,
                            CajeroId, Cajero);

                return View();
            }
        }

        // GET: HabitacionCliente/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Internamiento, Session))
            {
                this.AddNotification("No posees permisos para Editar Habitaciones por Clientes.", NotificationType.WARNING);
                return RedirectToAction("Index", "HabitacionCliente");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HabitacionCliente habitacionCliente = await db.HabitacionCliente.FindAsync(id);
            if (habitacionCliente == null)
            {
                return HttpNotFound();
            }

            getListData(habitacionCliente.HabitacionId, habitacionCliente._Habitacion.CodigoHabitacion,
                            habitacionCliente.ClienteId, string.Format("{0} {1}", habitacionCliente._Cliente.Nombre, habitacionCliente._Cliente.Apellido),
                           habitacionCliente.DoctorId, string.Format("{0} {1}", habitacionCliente._Doctor.Nombre, habitacionCliente._Doctor.Apellido),
                           habitacionCliente.EnfermeraId, string.Format("{0} {1}", habitacionCliente._Enfermera.Nombre, habitacionCliente._Enfermera.Apellido),
                           habitacionCliente.CajeroId, string.Format("{0} {1}", habitacionCliente._Cajero.Nombre, habitacionCliente._Cajero.Apellido));
            return View(habitacionCliente);
        }

        // POST: HabitacionCliente/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int Id, 
                                            int HabitacionId, string Habitacion,
                                            int ClienteId, string Cliente,
                                            int DoctorId, string Doctor,
                                            int EnfermeraId, string Enfermera,
                                            int CajeroId, string Cajero)
        {
            HabitacionCliente habitacionCliente = db.HabitacionCliente.Find(Id);

            if (!string.IsNullOrEmpty(Habitacion) && !string.IsNullOrEmpty(Cliente) &&
                !string.IsNullOrEmpty(Doctor) && !string.IsNullOrEmpty(Enfermera) &&
                !string.IsNullOrEmpty(Cajero))
            {                
                habitacionCliente.DoctorId = DoctorId;
                habitacionCliente.EnfermeraId = EnfermeraId;
                habitacionCliente.HabitacionId = HabitacionId;
                habitacionCliente.FechaModificacion = DateTime.Now;

                db.Entry(habitacionCliente).State = EntityState.Modified;
                await db.SaveChangesAsync();
                this.AddNotification("Asignacion de Habitacion al Paciente modificado exitosamente.", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }
            else
            {
                this.AddNotification("Es necesario completar todas las opciones", NotificationType.ERROR);
                getListData(HabitacionId, Habitacion, ClienteId, Cliente,
                            DoctorId, Doctor, EnfermeraId, Enfermera,
                            CajeroId, Cajero);

                return View(habitacionCliente);
            }
        }

        // GET: HabitacionCliente/Delete/5
        public async Task<ActionResult> Complete(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Internamiento, Session))
            {
                this.AddNotification("No posees permisos dar de Alta a pacientes", NotificationType.WARNING);
                return RedirectToAction("Index", "HabitacionCliente");
            }

            HabitacionCliente habitacionCliente = await db.HabitacionCliente.FindAsync(id);
            habitacionCliente.FechaModificacion = DateTime.Now;
            habitacionCliente.Estado = Models.Enums.EstadoEnum.Completa;
            db.Entry(habitacionCliente).State = EntityState.Modified;
            await db.SaveChangesAsync();
            this.AddNotification(string.Format("El paciente {0} {1}, se le ha dado de Alta", habitacionCliente._Cliente.Nombre, habitacionCliente._Cliente.Apellido), NotificationType.SUCCESS);
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

        private void getListData(int HabitacionId = 0, string Habitacion = "",
                                int ClienteId = 0, string Cliente = "",
                                int DoctorId = 0, string Doctor = "",
                                int EnfermeraId = 0, string Enfermera = "",
                                int CajeroId = 0, string Cajero = "") 
        {
            ViewBag.HabitacionId = HabitacionId;
            ViewBag.Habitacion = Habitacion;
            ViewBag.ClienteId = ClienteId;
            ViewBag.Cliente = Cliente;
            ViewBag.DoctorId = DoctorId;
            ViewBag.Doctor = Doctor;
            ViewBag.EnfermeraId = EnfermeraId;
            ViewBag.Enfermera = Enfermera;
            ViewBag.CajeroId = CajeroId;
            ViewBag.Cajero = Cajero;

            var habitacionesEnUso = db.HabitacionCliente.Where(a => a.Estado == Models.Enums.EstadoEnum.Activo).ToList();
            var habitaciones = db.Habitacion.Where(a => a.Estado == Models.Enums.EstadoEnum.Activo).Include(a=>a._EdificioNivel).ToList();
            var clientes = db.Cliente.Where(a => a.Estado == Models.Enums.EstadoEnum.Activo).ToList();
            List<Habitacion> habitacionesList = new List<Habitacion>();

            foreach (var hab in habitaciones)
            {
                int cantidad = habitacionesEnUso.Where(a => a.HabitacionId == hab.Id).Count();
                hab.MaximoClientes = (cantidad >= hab.MaximoClientes) ? 0 : hab.MaximoClientes - cantidad;

                if(hab.MaximoClientes > 0)
                    habitacionesList.Add(hab);
            }

            ViewBag.Habitaciones = habitacionesList;
            ViewBag.Clientes = clientes.Where(a => !habitacionesEnUso.Any(b => b.ClienteId == a.Id)).ToList();
            ViewBag.Doctores = db.Usuario.Where(a => a.Estado == Models.Enums.EstadoEnum.Activo && a.GrupoUsuarioId == 4).ToList();
            ViewBag.Enfermeras = db.Usuario.Where(a => a.Estado == Models.Enums.EstadoEnum.Activo && a.GrupoUsuarioId == 6).ToList();
            ViewBag.Cajeros = db.Usuario.Where(a => a.Estado == Models.Enums.EstadoEnum.Activo && a.GrupoUsuarioId == 7).ToList();

        }
    }
}
