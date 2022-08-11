
using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using MedicalApp.Context;
using MedicalApp.Models.Clientes;
using MedicalApp.Extensions;
using System.Linq;
using System.Collections.Generic;

namespace MedicalApp.Controllers.Clientes
{
    public class ClienteLoginController : Controller
    {
        private MainDbContext db = new MainDbContext();
        private Encriptar_DesEncriptar encriptar_DesEncriptar = new Encriptar_DesEncriptar();

        // GET: Login
        public ActionResult Login()
        {
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
                    var _cliente = await db.Cliente.FirstAsync(a => (a.ClienteId == cliente || a.Correo == cliente) && a.Estado == Models.Enums.EstadoEnum.Activo);

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

            ViewBag.usuario = cliente;
            ViewBag.contrasena = contrasena;
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
    }
}
