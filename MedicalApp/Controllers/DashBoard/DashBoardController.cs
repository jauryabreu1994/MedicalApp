using MedicalApp.Context;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;

namespace MedicalApp.Controllers.DashBoard
{
    public class DashBoardController : Controller
    {
        private MainDbContext db = new MainDbContext();
        private Encriptar_DesEncriptar encriptar_DesEncriptar = new Encriptar_DesEncriptar();
        public ActionResult Index()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Perfil, Session))
            {
                return RedirectToAction("Index", "Login");
            }

            List<Models.Citas.Cita> citas = new List<Models.Citas.Cita>();
            Models.DashBoard.DashBoard dashBoard = new Models.DashBoard.DashBoard();

            int UserID = Convert.ToInt32(encriptar_DesEncriptar.DesEncriptar(Session["UserID"].ToString()));

            if (Convert.ToInt32(encriptar_DesEncriptar.DesEncriptar(Session["GrupoUsuarioId"].ToString())) == 4)
            {
                citas = db.Cita.Include(s => s._Empresa).Include(s => s._AreaEspecialidad).Include(s => s._Usuario).Include(s => s._Cliente)
                                       .Where(a => a.UsuarioId == UserID && !a.Eliminado &&
                                              a.Estado != Models.Enums.EstadoCitaEnum.Completada &&
                                              a.Estado != Models.Enums.EstadoCitaEnum.PendientePago &&
                                              a.Estado != Models.Enums.EstadoCitaEnum.Cancelada).ToList();
            }
            else
            {
                var doctores = db.UsuarioAsociado.Where(a => a.AsistenteId == UserID).ToList();

                if (doctores.Count() > 0)
                {
                    foreach (var d in doctores)
                    {
                        foreach (var line in db.Cita.Include(s => s._Empresa).Include(s => s._AreaEspecialidad).Include(s => s._Usuario).Include(s => s._Cliente)
                                       .Where(a => a.UsuarioId == d.DoctorId && !a.Eliminado &&
                                              a.Estado != Models.Enums.EstadoCitaEnum.Completada &&
                                              a.Estado != Models.Enums.EstadoCitaEnum.PendientePago &&
                                              a.Estado != Models.Enums.EstadoCitaEnum.Cancelada).ToList())
                        {
                            citas.Add(line);
                        }
                    }
                }
                else
                {
                    citas = db.Cita.Include(s => s._Empresa).Include(s => s._AreaEspecialidad).Include(s => s._Usuario).Include(s => s._Cliente)
                                       .Where(a => a.Estado != Models.Enums.EstadoCitaEnum.Completada && !a.Eliminado &&
                                              a.Estado != Models.Enums.EstadoCitaEnum.PendientePago &&
                                              a.Estado != Models.Enums.EstadoCitaEnum.Cancelada).ToList();
                }
            }
            

            dashBoard.Cant_Citas = citas.Count;
            dashBoard.Cant_Consultas = citas.Where(a => a.TipoCita == Models.Enums.TipoCitaEnum.Consulta).Count();
            dashBoard.Cant_VerificacionAnalisis = citas.Where(a => a.TipoCita == Models.Enums.TipoCitaEnum.VerificacionAnalisis).Count();
            dashBoard.Cant_PendientePorConfirmar = citas.Where(a => a.Estado == Models.Enums.EstadoCitaEnum.Pendiente).Count();

            dashBoard.Citas = citas.Where(a => a.Estado == Models.Enums.EstadoCitaEnum.Pendiente).ToList();

            return View(dashBoard);
        }



    }
}