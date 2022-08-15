using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MedicalApp.Context;
using MedicalApp.Models.Citas;
using MedicalApp.Models.Clientes;
using MedicalApp.Models.Usuarios;
using MedicalApp.Controllers.Empresa;
using MedicalApp.Extensions;
using MedicalApp.Models.Enums;

namespace MedicalApp.Controllers.Citas
{
    public class CitaController : Controller
    {
        private MainDbContext db = new MainDbContext();
        public List<Cliente> clientes = new List<Cliente>();
        private Encriptar_DesEncriptar encriptar_DesEncriptar = new Encriptar_DesEncriptar();
        // GET: Cita
        public ActionResult Index()
        {

            if (!new GenericController().hasAccess(PermisoEnum.Citas, Session))
            {
                this.AddNotification("No posees permisos para el Listado de Citas.", NotificationType.WARNING);
                return RedirectToAction("Index", "DashBoard");
            }

            var cita = db.Cita.Include(c => c._AreaEspecialidad).Include(c => c._Cliente).Include(c => c._Empresa).Include(c => c._Usuario);

            int UserID = Convert.ToInt32(encriptar_DesEncriptar.DesEncriptar(Session["UserID"].ToString()));
            List<Cita> citas = new List<Cita>();
            
            if (Convert.ToInt32(encriptar_DesEncriptar.DesEncriptar(Session["GrupoUsuarioId"].ToString())) == 4)
            {
                citas = db.Cita.Include(s => s._Empresa).Include(s => s._AreaEspecialidad).Include(s => s._Usuario).Include(s => s._Cliente)
                                       .Where(a => a.UsuarioId == UserID && !a.Eliminado).ToList();
            }
            else
            {
                var doctores = db.UsuarioAsociado.Where(a => a.AsistenteId == UserID).Where(a => !a.Eliminado).ToList();

                if (doctores.Count() > 0)
                {
                    foreach (var d in doctores)
                    {
                        foreach (var line in db.Cita.Include(s => s._Empresa).Include(s => s._AreaEspecialidad).Include(s => s._Usuario).Include(s => s._Cliente)
                                       .Where(a => a.UsuarioId == d.DoctorId && !a.Eliminado &&
                                              a.Estado != Models.Enums.EstadoCitaEnum.Completada &&
                                              a.Estado != Models.Enums.EstadoCitaEnum.PendientePago).ToList())
                        {
                            citas.Add(line);
                        }
                    }
                }
                else
                {
                    citas = db.Cita.Include(s => s._Empresa).Include(s => s._AreaEspecialidad).Include(s => s._Usuario).Include(s => s._Cliente).Where(a => !a.Eliminado).ToList();
                }
            }
            return View(citas.OrderBy(a => a.FechaCita).ToList()); 
        }
        public async Task<ActionResult> Paciente(int id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Citas, Session))
            {
                this.AddNotification("No posees permisos para ver el Perfil del Paciente.", NotificationType.WARNING);
                return RedirectToAction("Index", "DashBoard");
            }

            var cita = await db.Cita.FindAsync(id);
            
            var paciente = await db.Cliente.FindAsync(cita.ClienteId);
            var clienteHistorial = db.ClienteHistorial.Where(a => a.UsuarioId == cita._Usuario.Id && !a.Eliminado).OrderByDescending(a=>a.FechaCreacion).Take(3).ToList();
            
            ViewBag.CitaId = id;
            ViewBag.Cita = cita;
            ViewBag.ClienteHistorial = clienteHistorial;
            ViewBag.Cliente = paciente;
            return View(new ClienteHistorial());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Paciente([Bind(Include = "Id,ClienteId,UsuarioId,Documentacion,Indicaciones,FechaCreacion,FechaModificacion,Estado,Eliminado")] ClienteHistorial clienteHistorial, int CitaId)
        {
            DateTime fecha = DateTime.Now.Date;
            if (ModelState.IsValid)
            {
                int UserID = Convert.ToInt32(new Encriptar_DesEncriptar().DesEncriptar(Session["UserID"].ToString()));
                clienteHistorial.UsuarioId = UserID;
                db.ClienteHistorial.Add(clienteHistorial);
                await db.SaveChangesAsync();
                this.AddNotification("Historial de Paciente registrado exitosamente.", NotificationType.SUCCESS);
                
                return RedirectToAction("Complete", "Cita", new { id = CitaId });
            }

            var cita = await db.Cita.FindAsync(CitaId);
            var paciente = await db.Cliente.FindAsync(cita.ClienteId);
            var clienteHistoriales = db.ClienteHistorial.Where(a => a.UsuarioId == cita._Usuario.Id && !a.Eliminado).ToList();

            ViewBag.Cita = cita;
            ViewBag.ClienteHistorial = clienteHistoriales;
            ViewBag.Cliente = paciente;

            ViewBag.Indicaciones = clienteHistorial.Indicaciones;
            ViewBag.Documentacion = clienteHistorial.Documentacion;
            this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);
            return View(clienteHistorial);
        }

        public async Task<ActionResult> Pacientes()
        {
            try
            {
                if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Citas, Session))
                {
                    this.AddNotification("No posees permisos para el Listado de Citas.", NotificationType.WARNING);
                    return RedirectToAction("Index", "DashBoard");
                }

                int UsuarioId = Convert.ToInt32(encriptar_DesEncriptar.DesEncriptar(Session["UserID"].ToString()));
                int GrupoUsuarioId = Convert.ToInt32(encriptar_DesEncriptar.DesEncriptar(Session["GrupoUsuarioId"].ToString()));
                var usuario = await db.Usuario.FindAsync(UsuarioId);

                List<Usuario> usuarios = new List<Usuario>();
                if (GrupoUsuarioId == 5)
                {
                    var listadoDoctores = db.UsuarioAsociado.Where(a => a.AsistenteId == usuario.Id);

                    foreach (var doctor in listadoDoctores)
                        usuarios.Add(await db.Usuario.FindAsync(doctor.DoctorId));
                }
                else if (GrupoUsuarioId == 4)
                    usuarios.Add(usuario);
                else
                {
                    this.AddNotification("Este perfil no es Doctor/a o Asistente.", NotificationType.WARNING);
                    return RedirectToAction("Index", "Cita");
                }

                List<Cita> citas = new List<Cita>();
                foreach (var u in usuarios)
                {
                    var cs = db.Cita.Include(s => s._Empresa).Include(s => s._AreaEspecialidad)
                                    .Include(s => s._Usuario).Include(s => s._Cliente).ToList()
                                    .Where(a => a.FechaCita.Date == DateTime.Now.Date && a.UsuarioId == u.Id &&
                                                a.Estado != EstadoCitaEnum.Completada &&
                                                a.Estado != EstadoCitaEnum.PendientePago).ToList();
                    foreach (var c in cs)
                        citas.Add(c);
                }
                return View(citas);
            }
            catch {
                this.AddNotification("Solo los medicos poseen aceeso a este panel.", NotificationType.WARNING);
                return RedirectToAction("Index", "DashBoard");
            }
        }

        public JsonResult Doctor_Bind(int areaEspecialidadId)
        {

            if (string.IsNullOrEmpty(areaEspecialidadId.ToString()))
            {
                return Json(null);
            }

            List<Usuario> usuarios = db.Usuario.Where(c => c.AreaEspecialidadId == areaEspecialidadId && c.Estado == Models.Enums.EstadoEnum.Activo && c.GrupoUsuarioId == 4).ToList();
            List<SelectListItem> listado = new List<SelectListItem>();

            foreach (var usuario in usuarios)
            {
                listado.Add(new SelectListItem { Text = string.Format("{0} {1}", usuario.Nombre, usuario.Apellido), Value = usuario.Id.ToString() });
            }

            return Json(listado, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Hora_Bind(int doctorId, string fechaId)
        {

            if (string.IsNullOrEmpty(doctorId.ToString()) || string.IsNullOrEmpty(fechaId.ToString()))
            {
                return Json(null);
            }
            DateTime fecha = Convert.ToDateTime(fechaId);

            List<SelectListItem> listado = new List<SelectListItem>();
            var doctor = db.Usuario.Find(doctorId);
            if (doctor.__UsuariosLicencias.Where(a => a.FechaLicencia.Date == fecha.Date).Count() == 0)
            {
                var horarioDisponible = doctor.__UsuariosHorarios.Where(a => a.Dia == (DiasEnum)Convert.ToInt32(fecha.DayOfWeek)).ToList();

                if (horarioDisponible.Count > 0)
                {
                    foreach (var hd in horarioDisponible)
                    {
                        if (doctor.__Citas.Where(a => a.FechaCita == fecha.Date.AddHours(hd._Horario.Hora).AddMinutes(hd._Horario.Minutos)).Count() < hd.CantidadPacientes)
                        {
                            if (hd._Horario.Hora > 12)
                                listado.Add(new SelectListItem { Text = string.Format("{0}:{1} pm", (hd._Horario.Hora - 12).ToString("00"), hd._Horario.Minutos.ToString("00")), Value = hd._Horario.Id.ToString() });
                            else
                                listado.Add(new SelectListItem { Text = string.Format("{0}:{1} am", hd._Horario.Hora.ToString("00"), hd._Horario.Minutos.ToString("00")), Value = hd._Horario.Id.ToString() });
                        }
                    }
                }
            }

            return Json(listado, JsonRequestBehavior.AllowGet);
        }

        // GET: Cita/Confirm
        public async Task<ActionResult> Confirm(int id)
        {
            var cita = db.Cita.Where(a => a.Id == id).First();
            if (cita.Estado != Models.Enums.EstadoCitaEnum.Cancelada)
            {
                cita.Estado = cita.Estado != Models.Enums.EstadoCitaEnum.Confirmada ? Models.Enums.EstadoCitaEnum.Confirmada : Models.Enums.EstadoCitaEnum.Pendiente;
                db.Entry(cita).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // GET: Cita/Confirm
        public async Task<ActionResult> Complete(int id)
        {
            var cita = db.Cita.Where(a => a.Id == id).First();
            cita.Estado = Models.Enums.EstadoCitaEnum.Completada;
            cita.FechaModificacion = DateTime.Now;
            db.Entry(cita).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return RedirectToAction("Index", "ClienteHistorial", new { id = cita.ClienteId });
        }

        // GET: Cita/Create
        public ActionResult Create()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Citas, Session))
            {
                this.AddNotification("No posees permisos para Crear Citas.", NotificationType.WARNING);
                return RedirectToAction("Index", "Cita");
            }

            ViewBag.AreaEspecialidadId = new SelectList(db.AreaEspecialidad, "Id", "Descripcion");
            ViewBag.ClienteId = new SelectList(db.Cliente, "Id", "ClienteId");
            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Nombre");
            ViewBag.UsuarioId = new SelectList(db.Usuario, "Id", "UsuarioId");

            ViewBag.Fecha = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.Clientes = db.Cliente.Where(a => a.Estado == Models.Enums.EstadoEnum.Activo).ToList();
            ViewBag.AreaEspecialidades = db.AreaEspecialidad.Where(a => a.Estado == Models.Enums.EstadoEnum.Activo && a.AreaGeneralId == 1).Include(a => a._AreaGeneral).ToList();
            return View();
        }

        // POST: Cita/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(int ClienteId, string Cliente, int AreaEspecialidadId, string AreaEspecialidad, int DoctorId, DateTime FechaCita, int Hora, string Comentario, int TipoCita)
        {
            Cita cita = new Cita();

            var horario = await db.Horario.FirstAsync(a => a.Id == Hora);

            
            cita = new Cita()
            {
                Id = 0,
                EmpresaId = 1,
                ClienteId = ClienteId,
                CitaId = new EmpresaController().GenerateNumber(true, Models.Enums.CompanyEnum.Cita).Item1,
                AreaEspecialidadId = AreaEspecialidadId,
                UsuarioId = DoctorId,
                Comentario = Comentario,
                TipoCita = (Models.Enums.TipoCitaEnum)TipoCita,
                FechaCita = FechaCita.Date.AddHours(horario.Hora).AddMinutes(horario.Minutos),
                FechaCreacion = DateTime.Now,
                FechaModificacion = DateTime.Now,
                Estado = Models.Enums.EstadoCitaEnum.Pendiente,
                Eliminado = false
            };
            var citas = db.Cita.Where(a => !a.Eliminado && a.ClienteId == ClienteId && a.AreaEspecialidadId == AreaEspecialidadId && a.FechaCita > DateTime.Now).Include(a=>a._Usuario).ToList();
            if (citas.Count() > 0) 
            {
                var c = citas.FirstOrDefault();
                string notificacion = string.Format("El cliente posee una cita para esta especialidad. Doctor/a: {0} {1}, en Fecha {2}", c._Usuario.Nombre, c._Usuario.Apellido, c.FechaCita);
                this.AddNotification(notificacion, NotificationType.ERROR);
            }
            else if (!string.IsNullOrEmpty(Comentario) && DateTime.Now < cita.FechaCita)
            {
                if (ModelState.IsValid)
                {
                    var doctor = await db.Usuario.FindAsync(DoctorId);
                    if (doctor.__UsuariosLicencias.Where(a => a.FechaLicencia.Date == FechaCita.Date).Count() == 0)
                    {
                        var horarioDisponible = doctor.__UsuariosHorarios.First(a => a.HorarioId == horario.Id && a.Dia == (DiasEnum)Convert.ToInt32(FechaCita.DayOfWeek));

                        if (horarioDisponible != null)
                        {
                            if (doctor.__Citas.Where(a => a.FechaCita == cita.FechaCita).Count() < horarioDisponible.CantidadPacientes)
                            {
                                db.Cita.Add(cita);
                                await db.SaveChangesAsync();
                                this.AddNotification("Cita registrada exitosamente", NotificationType.SUCCESS);
                                return RedirectToAction("Index");
                            }
                            else
                                this.AddNotification("El doctor/a tiene el maximo de pacientes para este horario (seleccione otro horario)", NotificationType.ERROR);
                        }
                        else
                            this.AddNotification("El doctor/a no labora en el horario especificado.", NotificationType.ERROR);
                    }
                    else
                        this.AddNotification("El doctor no esta disponible en la fecha especificada.", NotificationType.ERROR);
                }
                else
                    this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);
            }
            else
                this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);

            ViewBag.ClienteId = ClienteId;
            ViewBag.Cliente = Cliente;
            ViewBag.AreaEspecialidadId = AreaEspecialidadId;
            ViewBag.AreaEspecialidad = AreaEspecialidad;
            ViewBag.DoctorId = DoctorId;
            ViewBag.Fecha = FechaCita.ToString("yyyy-MM-dd"); ;
            ViewBag.Hora = Hora;
            ViewBag.Comentario = Comentario;

            ViewBag.Clientes = db.Cliente.Where(a => a.Estado == Models.Enums.EstadoEnum.Activo).ToList();
            ViewBag.AreaEspecialidades = db.AreaEspecialidad.Where(a => a.Estado == Models.Enums.EstadoEnum.Activo).Include(a => a._AreaGeneral).ToList();
            return View();
        }

        // GET: Cita/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Citas, Session))
            {
                this.AddNotification("No posees permisos para Editar Citas.", NotificationType.WARNING);
                return RedirectToAction("Index", "Cita");
            }

            if (id == null)
            {
                return RedirectToAction("Error", "Home");;
            }
            Cita cita = await db.Cita.FindAsync(id);
            if (cita == null)
            {
                return HttpNotFound();
            }
            ViewBag.AreaEspecialidadId = new SelectList(db.AreaEspecialidad, "Id", "Descripcion", cita.AreaEspecialidadId);
            ViewBag.ClienteId = new SelectList(db.Cliente, "Id", "ClienteId", cita.ClienteId);
            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Nombre", cita.EmpresaId);
            ViewBag.UsuarioId = new SelectList(db.Usuario, "Id", "UsuarioId", cita.UsuarioId);

            ViewBag.Id = cita.Id;
            ViewBag.ClienteId = cita.ClienteId;
            ViewBag.Cliente = string.Format("{0} {1}", cita._Cliente.Nombre, cita._Cliente.Apellido);
            ViewBag.AreaEspecialidadId = cita.AreaEspecialidadId;
            ViewBag.AreaEspecialidad = cita._AreaEspecialidad.Descripcion;
            ViewBag.DoctorId = cita.UsuarioId;
            ViewBag.Comentario = cita.Comentario;
            ViewBag.Fecha = cita.FechaCita.ToString("yyyy-MM-dd");
            ViewBag.Hora = cita.FechaCita.Hour;
            ViewBag.Minutos = cita.FechaCita.Minute;
            ViewBag.AreaEspecialidades = db.AreaEspecialidad.Where(a => a.Estado == Models.Enums.EstadoEnum.Activo).Include(a => a._AreaGeneral).ToList();
            return View(cita);
        }

        // POST: Cita/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int Id, int ClienteId, string Cliente, int AreaEspecialidadId, string AreaEspecialidad, int DoctorId, DateTime FechaCita, int Hora, int Minutos, string Comentario, int TipoCita)
        {
            var cita = db.Cita.Find(Id);

            if (ModelState.IsValid)
            {
                cita.AreaEspecialidadId = AreaEspecialidadId;
                cita.UsuarioId = DoctorId;
                cita.Comentario = Comentario;
                cita.TipoCita = (Models.Enums.TipoCitaEnum)TipoCita;
                cita.FechaCita = FechaCita.Date.AddHours(Hora).AddMinutes(Minutos);
                cita.FechaModificacion = DateTime.Now;
                cita.Estado = Models.Enums.EstadoCitaEnum.Pendiente;
                cita.Eliminado = false;

                db.Entry(cita).State = EntityState.Modified;
                await db.SaveChangesAsync();
                this.AddNotification("Cita editada exitosamente", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }

            ViewBag.AreaEspecialidadId = new SelectList(db.AreaEspecialidad, "Id", "Descripcion", cita.AreaEspecialidadId);
            ViewBag.ClienteId = new SelectList(db.Cliente, "Id", "ClienteId", cita.ClienteId);
            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Nombre", cita.EmpresaId);
            ViewBag.UsuarioId = new SelectList(db.Usuario, "Id", "UsuarioId", cita.UsuarioId);

            ViewBag.Id = cita.Id;
            ViewBag.ClienteId = ClienteId;
            ViewBag.Cliente = Cliente;
            ViewBag.AreaEspecialidadId = AreaEspecialidadId;
            ViewBag.AreaEspecialidad = AreaEspecialidad;
            ViewBag.DoctorId = DoctorId;
            ViewBag.Fecha = FechaCita.ToString("yyyy-MM-dd"); ;
            ViewBag.Hora = Hora;
            ViewBag.Minutos = Minutos;
            ViewBag.Comentario = Comentario;

            ViewBag.AreaEspecialidades = db.AreaEspecialidad.Where(a => a.Estado == Models.Enums.EstadoEnum.Activo).Include(a => a._AreaGeneral).ToList();
            this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);
            return View(cita);
        }

        // GET: Cita/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Citas, Session))
            {
                this.AddNotification("No posees permisos para Eliminar Citas.", NotificationType.WARNING);
                return RedirectToAction("Index", "Cita");
            }

            if (id == null)
            {
                return RedirectToAction("Error", "Home");;
            }
            Cita cita = await db.Cita.FindAsync(id);
            if (cita == null)
            {
                return HttpNotFound();
            }
            return View(cita);
        }

        // POST: Cita/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Cita cita = await db.Cita.FindAsync(id);
            cita.FechaModificacion = DateTime.Now;
            cita.Eliminado = true;
            cita.Estado = Models.Enums.EstadoCitaEnum.Cancelada;
            db.Entry(cita).State = EntityState.Modified;
            await db.SaveChangesAsync();
            this.AddNotification("Cita eliminada exitosamente", NotificationType.SUCCESS);
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
