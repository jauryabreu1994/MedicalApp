using MedicalApp.Context;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using MedicalApp.Extensions;
using System.Threading.Tasks;
using MedicalApp.Models.Citas;
using System.Collections.Generic;
using MedicalApp.Models.Usuarios;
using MedicalApp.Controllers.Empresa;
using MedicalApp.Models.Clientes;
using MedicalApp.Models.Enums;
using System.IO;
using System.Web.Hosting;
using MedicalApp.Models.Ubicaciones;

namespace MedicalApp.Controllers.Pacientes
{
    public class PacienteController : Controller
    {
        private MainDbContext db = new MainDbContext();
        private Encriptar_DesEncriptar encriptar_DesEncriptar = new Encriptar_DesEncriptar();

        // GET: Login
        public ActionResult Login()
        {
            /*ViewBag.cliente = "402-2393820-6";
            ViewBag.contrasena = "402-2393820-6";*/
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string cliente, string contrasena)
        {
            try
            {
                if (!string.IsNullOrEmpty(cliente) && !string.IsNullOrEmpty(contrasena))
                {
                    var _cliente = await db.Cliente.FirstAsync(a => (a.Identificacion.Replace("-", "").Replace(" ","") == cliente.Replace("-", "").Replace(" ", "") || a.Correo == cliente) && a.Estado == Models.Enums.EstadoEnum.Activo);

                    if (_cliente != null)
                    {
                        var _contrasena = await db.ClienteContrasena.FirstAsync(a => a.ClienteId == _cliente.Id && a.Estado == Models.Enums.EstadoEnum.Activo);
                        if (encriptar_DesEncriptar.DesEncriptar(_contrasena.Contraseña) == contrasena)
                        {


                            Session.Add("UserID", encriptar_DesEncriptar.Encriptar(_cliente.Id.ToString()));
                            Session["UserName"] = (!string.IsNullOrEmpty(_cliente.Apellido)) ? string.Format("{0} {1}", _cliente.Nombre.Split(' ')[0], _cliente.Apellido.Split(' ')[0]) : _cliente.Nombre;
                            Session["GrupoUsuarioId"] = encriptar_DesEncriptar.Encriptar("99");
                            Session["GrupoUsuario"] = null;
                            Session["EmpresaId"] = -1;
                            return UserDashBoard();
                        }
                        else
                            this.AddNotification("Contraseña incorrecta", NotificationType.ERROR);

                    }
                    else
                        this.AddNotification("Usuario o Correo incorrecto", NotificationType.ERROR);
                }
            }
            catch
            {
                this.AddNotification("Usuario y/o Contraseña incorrecto", NotificationType.ERROR);
            }

            ViewBag.cliente = cliente;
            ViewBag.contrasena = contrasena;
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(string nombre, string apellido, string identificacion, string telefono,
                                                 string genero, string correo, string contrasena, string rcontrasena)
        {
            if (!string.IsNullOrEmpty(nombre) && !string.IsNullOrEmpty(apellido) && !string.IsNullOrEmpty(identificacion) &&
                !string.IsNullOrEmpty(genero) && !string.IsNullOrEmpty(correo) && !string.IsNullOrEmpty(telefono) &&
                !string.IsNullOrEmpty(contrasena) && !string.IsNullOrEmpty(rcontrasena))
            {
                if (contrasena != rcontrasena)
                    this.AddNotification("Contraseña no coinciden", NotificationType.ERROR);
                else
                {
                    Cliente cliente = new Cliente() {
                        ClienteId = new EmpresaController().GenerateNumber(true, CompanyEnum.Cliente).Item1,
                        Identificacion = new GenericController().SetFormatVatNumber(identificacion),
                        Nombre = nombre,
                        Apellido = apellido,
                        Genero = (GeneroEnum)Convert.ToInt32(genero),
                        Correo = correo,
                        Direccion = "",
                        CiudadId = 979,
                        PaisId = 1,
                        Telefono = telefono,
                        Estado = EstadoEnum.Activo,
                        Eliminado = false,
                        FechaCreacion = DateTime.Now,
                        FechaModificacion = DateTime.Now,
                        Id = 0,
                        NombreFiscal = string.Format("{0} {1}", nombre, apellido)
                    };
                    cliente = db.Cliente.Add(cliente);

                    await db.SaveChangesAsync();
                    ClienteContrasena clienteContrasena = new ClienteContrasena()
                    {
                        Id = 0,
                        ClienteId = cliente.Id,
                        Contraseña = new Encriptar_DesEncriptar().Encriptar(contrasena),
                        FechaModificacion = DateTime.Now,
                        FechaCreacion = DateTime.Now,
                        Eliminado = false,
                        Estado = EstadoEnum.Activo

                    };
                    db.ClienteContrasena.Add(clienteContrasena);
                    await db.SaveChangesAsync();
                    this.AddNotification("Paciente registrado exitosamente.", NotificationType.SUCCESS);

                    return await Login(correo, contrasena);
                }
            }
            else
                this.AddNotification("Completar todos los campos", NotificationType.ERROR);

            ViewBag.nombre = nombre;
            ViewBag.apellido = apellido;
            ViewBag.identificacion = identificacion;
            ViewBag.genero = genero;
            ViewBag.correo = correo;
            ViewBag.telefono = telefono;
            ViewBag.contrasena = contrasena;
            ViewBag.confirmarContrasena = rcontrasena;
            return View();
        }

        public ActionResult UserDashBoard()
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Index", "Paciente");
            }
            else
            {
                this.AddNotification("Correo y/o Contraseña incorrecto", NotificationType.ERROR);
                return View();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult Index()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Perfil, Session, false))
            {
                return RedirectToAction("Login", "Paciente");
            }

            Models.DashBoard.DashBoard dashBoard = new Models.DashBoard.DashBoard();
            int ClienteId = Convert.ToInt32(encriptar_DesEncriptar.DesEncriptar(Session["UserID"].ToString()));

            var citas = db.Cita.Include(s => s._Empresa).Include(s => s._AreaEspecialidad).Include(s => s._Usuario).Include(s => s._Cliente)
                               .Where(a => a.ClienteId == ClienteId &&  a.Estado != Models.Enums.EstadoCitaEnum.Cancelada).ToList();

            dashBoard.Cant_Citas = citas.Count;
            dashBoard.Cant_PendientePorConfirmar = citas.Where(a => a.Estado == Models.Enums.EstadoCitaEnum.Pendiente).Count();
            dashBoard.Cant_Consultas = citas.Where(a => a.Estado == Models.Enums.EstadoCitaEnum.Confirmada).Count();
            var listadoCitas = citas.Where(a => a.Estado != Models.Enums.EstadoCitaEnum.Pendiente ||
                                                a.Estado != Models.Enums.EstadoCitaEnum.Confirmada).OrderBy(a => a.FechaCita);


            dashBoard.Proxima_Cita = (listadoCitas.Count() > 0) ?
                 listadoCitas.FirstOrDefault().FechaCita.ToString("dd/MM/yyyy") : "No Citas";

            dashBoard.Citas = citas.Where(a => a.Estado != Models.Enums.EstadoCitaEnum.Pendiente ||
                                               a.Estado != Models.Enums.EstadoCitaEnum.Confirmada).ToList();

            return View(dashBoard);
        }

        public ActionResult Citas()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Citas, Session, false))
            {
                return RedirectToAction("Login", "Paciente");
            }

            List<Cita> cita = new List<Cita>();
            int ClienteId = Convert.ToInt32(encriptar_DesEncriptar.DesEncriptar(Session["UserID"].ToString()));

            var citas = db.Cita.Include(s => s._Empresa).Include(s => s._AreaEspecialidad).Include(s => s._Usuario).Include(s => s._Cliente)
                               .Where(a => a.ClienteId == ClienteId).OrderBy(a=>a.FechaCita).ToList();

            return View(citas);
        }

        public JsonResult Doctor_Bind(int areaEspecialidadId, int empresaId)
        {

            if (string.IsNullOrEmpty(areaEspecialidadId.ToString()))
            {
                return Json(null);
            }

            List<Usuario> usuarios = db.Usuario.Where(c => c.AreaEspecialidadId == areaEspecialidadId && 
                                                           c.Estado == Models.Enums.EstadoEnum.Activo && 
                                                           c.EmpresaId == empresaId &&
                                                           c.GrupoUsuarioId == 4).ToList();
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

        public ActionResult Create()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Citas, Session, false))
            {
                this.AddNotification("No posees permisos para Crear Citas.", NotificationType.WARNING);
                return RedirectToAction("Login", "Paciente");
            }
            ViewBag.AreaEspecialidadId = new SelectList(db.AreaEspecialidad, "Id", "Descripcion");
            ViewBag.ClienteId = new SelectList(db.Cliente, "Id", "ClienteId");
            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Nombre");
            ViewBag.UsuarioId = new SelectList(db.Usuario, "Id", "UsuarioId");

            ViewBag.Fecha = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.Hora = 8;
            ViewBag.Minutos = 0;
            ViewBag.Empresas = db.Empresa.Where(a => a.Estado == Models.Enums.EstadoEnum.Activo).ToList();
            ViewBag.AreaEspecialidades = db.AreaEspecialidad.Where(a => a.Estado == Models.Enums.EstadoEnum.Activo && a.AreaGeneralId == 1).Include(a => a._AreaGeneral).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(int EmpresaId, string Empresa, int AreaEspecialidadId, string AreaEspecialidad, 
                                               int DoctorId, DateTime FechaCita, int Hora, int TipoCita, string Comentario = "")
        {
            Cita cita = new Cita();
            int ClienteId = Convert.ToInt32(encriptar_DesEncriptar.DesEncriptar(Session["UserID"].ToString()));
            var horario = await db.Horario.FirstAsync(a => a.Id == Hora);
            cita = new Cita()
            {
                Id = 0,
                EmpresaId = EmpresaId,
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
            if (!string.IsNullOrEmpty(Comentario) && DateTime.Now <= cita.FechaCita)
            {
                if (ModelState.IsValid)
                {
                    var doctor = await db.Usuario.FindAsync(DoctorId);
                    if (doctor.__UsuariosLicencias.Where(a => a.FechaLicencia.Date == FechaCita.Date).Count() == 0) {
                        var horarioDisponible = doctor.__UsuariosHorarios.First(a => a.HorarioId == horario.Id && a.Dia == (DiasEnum)Convert.ToInt32(FechaCita.DayOfWeek));

                        if (horarioDisponible != null)
                        {
                            if (doctor.__Citas.Where(a => a.FechaCita == cita.FechaCita).Count() < horarioDisponible.CantidadPacientes)
                            {
                                db.Cita.Add(cita);
                                await db.SaveChangesAsync();
                                this.AddNotification("Cita registrado exitosamente", NotificationType.SUCCESS);
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

            ViewBag.EmpresaId = EmpresaId;
            ViewBag.Empresa = Empresa;
            ViewBag.AreaEspecialidadId = AreaEspecialidadId;
            ViewBag.AreaEspecialidad = AreaEspecialidad;
            ViewBag.DoctorId = DoctorId;
            ViewBag.Fecha = FechaCita.ToString("yyyy-MM-dd"); ;
            ViewBag.Hora = Hora;
            ViewBag.Comentario = Comentario;

            ViewBag.Empresas = db.Empresa.Where(a => a.Estado == Models.Enums.EstadoEnum.Activo).ToList();
            ViewBag.AreaEspecialidades = db.AreaEspecialidad.Where(a => a.Estado == Models.Enums.EstadoEnum.Activo && a.AreaGeneralId == 1).Include(a => a._AreaGeneral).ToList();
            
            return View();
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Citas, Session, false))
            {
                this.AddNotification("No posees permisos para Editar Citas.", NotificationType.WARNING);
                return RedirectToAction("Login", "Paciente");
            }

            if (id == null)
            {
                return RedirectToAction("Error", "Home"); ;
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
            ViewBag.EmpresaId = cita.EmpresaId;
            ViewBag.Empresa = string.Format("{0}, RNC: {1}", cita._Empresa.Nombre, cita._Cliente.Identificacion);
            ViewBag.AreaEspecialidadId = cita.AreaEspecialidadId;
            ViewBag.AreaEspecialidad = cita._AreaEspecialidad.Descripcion;
            ViewBag.DoctorId = cita.UsuarioId;
            ViewBag.Doctor = string.Format("{0} {1}", cita._Usuario.Nombre, cita._Usuario.Apellido);
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
        public async Task<ActionResult> Edit(int Id, int EmpresaId, string Empresa, int AreaEspecialidadId, string AreaEspecialidad, int DoctorId, string Doctor, DateTime FechaCita, int Hora, int Minutos, string Comentario, int TipoCita)
        {
            var cita = db.Cita.Find(Id);

            if (ModelState.IsValid)
            {
                cita.Comentario = Comentario;
                cita.TipoCita = (Models.Enums.TipoCitaEnum)TipoCita;
                cita.FechaModificacion = DateTime.Now;
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
            ViewBag.EmpresaId = EmpresaId;
            ViewBag.Empresa = Empresa;
            ViewBag.AreaEspecialidadId = AreaEspecialidadId;
            ViewBag.AreaEspecialidad = AreaEspecialidad;
            ViewBag.DoctorId = DoctorId;
            ViewBag.Doctor = Doctor;
            ViewBag.Fecha = FechaCita.ToString("yyyy-MM-dd"); ;
            ViewBag.Hora = Hora;
            ViewBag.Minutos = Minutos;
            ViewBag.Comentario = Comentario;

            ViewBag.AreaEspecialidades = db.AreaEspecialidad.Where(a => a.Estado == Models.Enums.EstadoEnum.Activo).Include(a => a._AreaGeneral).ToList();
            this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);
            return View(cita);
        }


        public async Task<ActionResult> Delete(int id)
        {
            var cita = await db.Cita.FindAsync(id);
            cita.FechaModificacion = DateTime.Now;
            cita.Eliminado = true;
            cita.Estado = Models.Enums.EstadoCitaEnum.Cancelada;
            db.Entry(cita).State = EntityState.Modified;
            await db.SaveChangesAsync();
            this.AddNotification("Cita cancelada exitosamente", NotificationType.SUCCESS);
           
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Historial()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Clientes, Session, false))
            {
                return RedirectToAction("Login", "Paciente");
            }

            Models.DashBoard.DashBoard dashBoard = new Models.DashBoard.DashBoard();
            int ClienteId = Convert.ToInt32(encriptar_DesEncriptar.DesEncriptar(Session["UserID"].ToString()));

            var clienteHistorial = db.ClienteHistorial.Where(a => a.ClienteId == ClienteId).Include(c => c._Cliente).Include(c => c._Usuario);

            ViewBag.Id = ClienteId;
            return View(await clienteHistorial.ToListAsync());
        }
        //invoice
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Historial(int invoice)
        {
            Models.DashBoard.DashBoard dashBoard = new Models.DashBoard.DashBoard();
            int ClienteId = Convert.ToInt32(encriptar_DesEncriptar.DesEncriptar(Session["UserID"].ToString()));

            var clienteHistorial = db.ClienteHistorial.Where(a => a.ClienteId == ClienteId).Include(c => c._Cliente).Include(c => c._Usuario);
            string report = await GenerarReporte(invoice);

            ViewBag.Id = ClienteId;
            ViewBag.File = report;
            return View(await clienteHistorial.ToListAsync());
        }

        public ActionResult Salir()
        {
            Session.Abandon();
            Session.Clear();
            Response.Cookies.Clear();

            return RedirectToAction("Login");

        }

        private async Task<string> GenerarReporte(int id)
        {
            var historial = await db.ClienteHistorial.FindAsync(id);
            var empresa = await db.Empresa.FindAsync(historial._Usuario.EmpresaId);

            string html = @"<!DOCTYPE html>
                                            <html lang='en'>
                                              <head>
                                                <meta charset='utf-8'>
                                                <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                                                <meta name='viewport' content='width=device-width, initial-scale=1, shrink-to-fit=no'>
                                                <meta name='description' content=''>
                                                <meta name='author' content=''>
                                                <link rel='icon' type='image/png' href='img/favicom-nj.png'>
                                                <title>Indicaciones del Paciente</title>
                                                <link href='https://fonts.googleapis.com/css2?family=Lexend:wght@100;400;500&display=swap' rel='stylesheet'>
                                              </head>
                                              <style type='text/css'>
                                                p {
                                                  font-size: 14px;
                                                }

                                                #footer {
                                                  position: fixed;
                                                  padding: 10px 10px 0px 10px;
                                                  bottom: 0;
                                                  width: 100%;
                                                }

                                                body {
                                                  -webkit-print-color-adjust: exact !important;
                                                }
                                              </style>
                                              <body style='background: linear-gradient(to bottom, #d6d6d6, #dedcdc);font-family: Lexend, sans-serif;'>
                                                <div class='contenedor-invoice' style='margin-top: 3%;background: #fff;width: 90%; padding: 10px 27px 10px 27px; margin: 0 auto;border-top: 5px #c01419 solid;border-radius: 5px;'>
                                                  <div class='logo-invoice'>
                                                    <br />
                                                  </div>
                                                  <div class='logo-invoice' style='display: inline-flex;width: 100%;'>
                                                    <div style='width: 100%;'>
                                                      <br />
                                                      <p style='text-align: right'><br /><br />
                                                        "+ historial.FechaCreacion.ToString("dd/MM/yyyy hh:mm:ss tt") + @" <br />
                                                        <b>"+ empresa.Nombre + @"</b> <br />
                                                        <b>RNC:</b> " + empresa.Identificacion + @" <br />
                                                        <b> Tel.:</b> " + empresa.Telefono + @" <br />
                                                        " + empresa.Direccion + @" <br />
                                                        " + string.Format("{0}, {1}", empresa._Ciudad.Descripcion, empresa._Pais.Descripcion) + @" <br />
                                                      </p>
                                                    </div>
                                                  </div>
                                                  <br />
                                                  <br /><br />
                                                    <h3 style='text-align: center'>Indicaciones del Paciente</h3>
                                                    <br />
                                                  <div class='logo-invoice' style='display: inline-flex;width: 100%;'>
                                                    <div style='width: 95%;'>
                                                      <p>
                                                          <b>Paciente: </b> "+ string.Format("{0} {1}", historial._Cliente.Nombre, historial._Cliente.Apellido) + @" <br>
                                                          <b>Identificacion: </b> " + historial._Cliente.Identificacion + @" <br>
                                                          <b>Edad: </b> {9} <br>
                                                          <b>Genero: </b> " + historial._Cliente.Genero.ToString() + @"  <br>
                                                          <b>Telefono: </b> " + historial._Cliente.Telefono + @"  <br><br>
                                                      </p>
                                                      <p>
                                                        <b>Revision: </b> " + historial.Documentacion + @"  <br><br>
                                                        <b>Indicaciones: </b> " + historial.Indicaciones + @"  <br><br>
                                                      </p>
            
           
                                                    </div>
                                                    <div style='width: 5%;'></div>
                                                  </div>

                                                  <br />
                                                  <br />
                                                  <footer id='footer' style='background: #fff;width: 100%;'>
                                                    <div style='margin-top: 2%;width: 85%;display: inline-block;text-align: center;font-size: 14px;line-height: 21px;color: #656565;'>
                                                      <p>
                                                          ________________________________<br><br>
                                                          <b>Doctor/a: </b> " + string.Format("{0} {1}", historial._Usuario.Nombre, historial._Usuario.Apellido) +@"  <br>
                                                          <b>Exequátur: </b> {15} <br>
                                                          <b>Telefono: </b> " + historial._Usuario.Telefono + @" <br><br><br><br>
                                                      </p>
            
                                                        <p>
                                                          " + empresa.Nombre + @" <br/>
                                                        <b>RNC: </b> " + empresa.Identificacion + @" <b>Tel.:</b>" + empresa.Telefono + @"
                                             <br>  " + string.Format("{0}, {1}, {2}", empresa.Direccion, empresa._Ciudad.Descripcion, empresa._Pais.Descripcion) + @" 

                                                      </p>
                                                    </div>
                                                  </footer>
                                                </div>
                                              </body>
                                            </html>";

            
             GenerateHTMLFile(html, string.Format("{0}-{1}-{2}", historial.ClienteId, historial.UsuarioId, historial.Id));
            return html;
        }


        private string GenerateHTMLFile(string html, string code)
        {
           string urlPath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "Report");
            try
            {
                if (!Directory.Exists(urlPath))
                    Directory.CreateDirectory(urlPath);

                DeleteOldFile(urlPath);

                urlPath += @"\" + code + ".html";

                using (StreamWriter sw = System.IO.File.CreateText(urlPath))
                {
                    sw.WriteLine(html);
                }

                return "../Report/" + code + ".html";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }

        private void DeleteOldFile(string path)
        {
            try
            {
                string[] files = Directory.GetFiles(path);

                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.LastAccessTime < DateTime.Now.Date.AddDays(-2))
                        fi.Delete();
                }
            }
            catch
            {
            }
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

        public ActionResult Perfil()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Perfil, Session, false))
            {
                return RedirectToAction("Login", "Paciente");
            }

            Models.DashBoard.DashBoard dashBoard = new Models.DashBoard.DashBoard();
            int clienteId = Convert.ToInt32(encriptar_DesEncriptar.DesEncriptar(Session["UserID"].ToString()));

            var cliente = db.Cliente.Find(clienteId);
            ViewBag.CiudadId = new SelectList(db.Ciudad, "Id", "Descripcion", cliente.CiudadId);
            ViewBag.PaisId = new SelectList(db.Pais, "Id", "Descripcion", cliente.PaisId);

            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Perfil(int Id, string Nombre, string Apellido, string Identificacion, string NombreFiscal,
                                               int PaisId, int CiudadId, string Direccion, string Correo, string Telefono, int Genero, DateTime FechaNacimiento)
        {
            var cliente = db.Cliente.Find(Id);

            if (ModelState.IsValid)
            {
                cliente.Nombre = Nombre;
                cliente.Apellido = Apellido;
                cliente.Identificacion = Identificacion;
                cliente.NombreFiscal = NombreFiscal;
                cliente.PaisId = PaisId;
                cliente.CiudadId = CiudadId;
                cliente.Direccion = Direccion;
                cliente.Correo = Correo;
                cliente.Telefono = Telefono;
                cliente.Genero = (GeneroEnum)Genero;
                cliente.FechaModificacion = DateTime.Now;
                cliente.FechaNacimiento = FechaNacimiento;
                cliente.Eliminado = false;

                db.Entry(cliente).State = EntityState.Modified;
                await db.SaveChangesAsync();
                this.AddNotification("Perfil actualizado exitosamente", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }

            ViewBag.CiudadId = new SelectList(db.Ciudad, "Id", "Descripcion", cliente.CiudadId);
            ViewBag.PaisId = new SelectList(db.Pais, "Id", "CodigoArea", cliente.PaisId);

            this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);
            return View(cliente);
        }

    }
}