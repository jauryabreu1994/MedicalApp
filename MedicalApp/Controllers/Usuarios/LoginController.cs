using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using MedicalApp.Context;
using MedicalApp.Extensions;
using System.Linq;
using System.Collections.Generic;
using System;

namespace MedicalApp.Controllers.Usuarios
{
    public class LoginController : Controller
    {
        private MainDbContext db = new MainDbContext();
        private Encriptar_DesEncriptar encriptar_DesEncriptar = new Encriptar_DesEncriptar();
        // GET: Login
        public ActionResult Index()
        {
            /*ViewBag.usuario = "mcoster";
            ViewBag.contrasena = "master";*/
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(int? empresaId, string usuario, string contrasena)
        {
            try
            {
                if (empresaId == 0)
                {
                    ViewBag.CompanyError = "Debe seleccionar una empresa";
                }
                else if (!string.IsNullOrEmpty(usuario) && !string.IsNullOrEmpty(contrasena))
                {
                    var _usuario = await db.Usuario.FirstAsync(a => (a.UsuarioId == usuario || a.Correo == usuario) && a.EmpresaId == empresaId && a.Estado == Models.Enums.EstadoEnum.Activo);

                    if (_usuario != null)
                    {
                        var _contrasena = await db.UsuarioContrasena.FirstAsync(a => a.UsuarioId == _usuario.Id && a.Estado == Models.Enums.EstadoEnum.Activo);
                        if (encriptar_DesEncriptar.DesEncriptar(_contrasena.Contraseña) == contrasena)
                        {


                            Session.Add("UserID", encriptar_DesEncriptar.Encriptar(_usuario.Id.ToString()));
                            Session["UserName"] = (!string.IsNullOrEmpty(_usuario.Apellido)) ? string.Format("{0} {1}", _usuario.Nombre.Split(' ')[0], _usuario.Apellido.Split(' ')[0]) : _usuario.Nombre;
                            Session["GrupoUsuarioId"] = encriptar_DesEncriptar.Encriptar(_usuario.GrupoUsuarioId.ToString());
                            Session["GrupoUsuario"] = db.GrupoUsuario.Find(_usuario.GrupoUsuarioId).Descripcion;
                            Session["EmpresaId"] = _usuario.EmpresaId;

                            int[] permisos = new int[db.Permiso.Count()];
                            int i = 0;
                            foreach (var p in _usuario._GrupoUsuario.__GrupoPermisos)
                            {
                                permisos[i] = p.PermisoId;
                                i++;
                            }
                            Session["PermisosId"] = permisos;

                            return UserDashBoard();
                        }
                        else
                            ViewBag.PasswordError = "Contraseña incorrecta";

                    }
                    else
                        ViewBag.UserError = "Usuario o Correo incorrecto";
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                ViewBag.UserError = (ex.Message == "Sequence contains no elements") ? "Usuario o Correo incorrecto" : "";
                ViewBag.GeneralError = (string.IsNullOrEmpty(ViewBag.UserError)) ? "Usuario y/o Contraseña incorrecto" : "";
            }

            ViewBag.usuario = usuario;
            ViewBag.contrasena = contrasena;
            return View();
        }

        public JsonResult Empresa_Bind()
        {
            List<Models.Empresas.Empresa> empresas = db.Empresa.Where(c => c.Estado == Models.Enums.EstadoEnum.Activo).ToList();
            List<SelectListItem> listado = new List<SelectListItem>();

            foreach (var ciudad in empresas)
                listado.Add(new SelectListItem { Text = ciudad.Nombre, Value = ciudad.Id.ToString() });

            return Json(listado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserDashBoard()
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Index", "DashBoard");
            }
            else
            {
                ViewBag.GeneralError = "Usuario y/o Contraseña incorrecto";
                return View();
            }
        }

        public ActionResult Salir() 
        {
            Session.Abandon();
            Session.Clear();
            Response.Cookies.Clear();

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
