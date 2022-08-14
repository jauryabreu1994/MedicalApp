using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MedicalApp.Context;
using MedicalApp.Controllers.Empresa;
using MedicalApp.Extensions;
using MedicalApp.Models.Ubicaciones;
using MedicalApp.Models.Usuarios;

namespace MedicalApp.Controllers.Usuarios
{
    public class UsuarioController : Controller
    {
        private MainDbContext db = new MainDbContext();
        // GET: Usuario
        public ActionResult Index()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Usuario, Session))
            {
                this.AddNotification("No posees permisos para el Listado de Usuarios.", NotificationType.WARNING);
                return RedirectToAction("Index", "DashBoard");
            }
            var usuario = db.Usuario.Include(u => u._AreaEspecialidad).Include(u => u._Ciudad).Include(u => u._Empresa).Include(u => u._GrupoUsuario).Include(u => u._Pais);
            ViewBag.ListGroup = db.GrupoUsuario.ToList();
            return View(usuario.OrderBy(a => a.Nombre).ToList());
        }

        public ActionResult Filter(int groupId = 1)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Usuario, Session))
            {
                this.AddNotification("No posees permisos para el Listado de Usuarios.", NotificationType.WARNING);
                return RedirectToAction("Index", "DashBoard");
            }
            var usuario = db.Usuario.Include(u => u._AreaEspecialidad).Include(u => u._Ciudad).Include(u => u._Empresa).Include(u => u._GrupoUsuario).Include(u => u._Pais).Where(a=>a.GrupoUsuarioId == groupId).ToList();
            ViewBag.ListGroup = db.GrupoUsuario.ToList();
            ViewBag.Grupo = usuario.Count() > 0 ?  usuario.FirstOrDefault()._GrupoUsuario.Descripcion : db.GrupoUsuario.ToList().Where(a=>a.Id == groupId).FirstOrDefault().Descripcion;
            
            return View(usuario);
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

        // GET: Usuario/Create
        public ActionResult Create()
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Usuario, Session))
            {
                this.AddNotification("No posees permisos para Crear Usuarios.", NotificationType.WARNING);
                return RedirectToAction("Index", "Usuario");
            }
            ViewBag.AreaEspecialidadId = new SelectList(db.AreaEspecialidad, "Id", "Descripcion");
            ViewBag.CiudadId = new SelectList(db.Ciudad, "Id", "Descripcion");
            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Nombre");
            ViewBag.GrupoUsuarioId = new SelectList(db.GrupoUsuario, "Id", "Descripcion");
            ViewBag.PaisId = new SelectList(db.Pais, "Id", "Descripcion");
            return View();
        }

        // POST: Usuario/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EmpresaId,GrupoUsuarioId,AreaEspecialidadId,UsuarioId,Nombre,Apellido,Identificacion,PaisId,CiudadId,Direccion,Correo,Telefono,Genero,FechaCreacion,FechaModificacion,Estado,Eliminado")] Usuario usuario)
        {
            if (new GenericController().IdentificationExist(usuario.Identificacion, false))
            {
                ViewBag.AreaEspecialidadId = new SelectList(db.AreaEspecialidad, "Id", "Descripcion", usuario.AreaEspecialidadId);
                ViewBag.CiudadId = new SelectList(db.Ciudad, "Id", "Descripcion", usuario.CiudadId);
                ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Nombre", usuario.EmpresaId);
                ViewBag.GrupoUsuarioId = new SelectList(db.GrupoUsuario, "Id", "Descripcion", usuario.GrupoUsuarioId);
                ViewBag.PaisId = new SelectList(db.Pais, "Id", "Descripcion", usuario.PaisId);
                this.AddNotification("Existe un usuario con esta cedula", NotificationType.ERROR);
                return View(usuario);
            }
            else if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(usuario.UsuarioId)) {

                    Models.Enums.CompanyEnum companyEnum = usuario.GrupoUsuarioId == 4 ? Models.Enums.CompanyEnum.Doctor : Models.Enums.CompanyEnum.Administrativo;
                    companyEnum = usuario.GrupoUsuarioId == 6 ? Models.Enums.CompanyEnum.Enfermera : companyEnum;
                    
                    usuario.UsuarioId = new EmpresaController().GenerateNumber(true, companyEnum).Item1; 
                }

                usuario.Identificacion = new GenericController().SetFormatVatNumber(usuario.Identificacion);
                usuario.Telefono = new GenericController().SetFormatPhoneNumer(usuario.Telefono);

                usuario.FechaModificacion = DateTime.Now;
                usuario.FechaCreacion = DateTime.Now;
                db.Usuario.Add(usuario);
                db.SaveChanges();

                #region Password
                UsuarioContrasena contrasena = new UsuarioContrasena()
                {
                    Id = 0,
                    UsuarioId = usuario.Id,
                    Estado = Models.Enums.EstadoEnum.Activo,
                    Eliminado = false,
                    FechaModificacion = DateTime.Now,
                    FechaCreacion = DateTime.Now,
                    Contraseña = new Encriptar_DesEncriptar().Encriptar("123456789")
                };

                db.UsuarioContrasena.Add(contrasena);
                db.SaveChanges();
                #endregion


                this.AddNotification("Usuario registrado exitosamente", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }

            ViewBag.AreaEspecialidadId = new SelectList(db.AreaEspecialidad, "Id", "Descripcion", usuario.AreaEspecialidadId);
            ViewBag.CiudadId = new SelectList(db.Ciudad, "Id", "Descripcion", usuario.CiudadId);
            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Nombre", usuario.EmpresaId);
            ViewBag.GrupoUsuarioId = new SelectList(db.GrupoUsuario, "Id", "Descripcion", usuario.GrupoUsuarioId);
            ViewBag.PaisId = new SelectList(db.Pais, "Id", "Descripcion", usuario.PaisId);
            this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);
            return View(usuario);
        }

        // GET: Usuario/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Usuario, Session))
            {
                this.AddNotification("No posees permisos para Editar Usuarios.", NotificationType.WARNING);
                return RedirectToAction("Index", "Usuario");
            }
            if (id == null)
            {
                return RedirectToAction("Error", "Home");;
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            ViewBag.AreaEspecialidadId = new SelectList(db.AreaEspecialidad, "Id", "Descripcion", usuario.AreaEspecialidadId);
            ViewBag.CiudadId = new SelectList(db.Ciudad, "Id", "Descripcion", usuario.CiudadId);
            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Nombre", usuario.EmpresaId);
            ViewBag.GrupoUsuarioId = new SelectList(db.GrupoUsuario, "Id", "Descripcion", usuario.GrupoUsuarioId);
            ViewBag.PaisId = new SelectList(db.Pais, "Id", "Descripcion", usuario.PaisId);
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EmpresaId,GrupoUsuarioId,AreaEspecialidadId,UsuarioId,Nombre,Apellido,Identificacion,PaisId,CiudadId,Direccion,Correo,Telefono,Genero,FechaCreacion,FechaModificacion,Estado,Eliminado")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.Identificacion = new GenericController().SetFormatVatNumber(usuario.Identificacion);
                usuario.Telefono = new GenericController().SetFormatPhoneNumer(usuario.Telefono);
                usuario.FechaModificacion = DateTime.Now;
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                this.AddNotification("Usuario modificado exitosamente", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }
            ViewBag.AreaEspecialidadId = new SelectList(db.AreaEspecialidad, "Id", "Descripcion", usuario.AreaEspecialidadId);
            ViewBag.CiudadId = new SelectList(db.Ciudad, "Id", "Descripcion", usuario.CiudadId);
            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Nombre", usuario.EmpresaId);
            ViewBag.GrupoUsuarioId = new SelectList(db.GrupoUsuario, "Id", "Descripcion", usuario.GrupoUsuarioId);
            ViewBag.PaisId = new SelectList(db.Pais, "Id", "Descripcion", usuario.PaisId);
            this.AddNotification("Favor completar todos los campos", NotificationType.ERROR);
            return View(usuario);
        }

        // GET: Usuario/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!new GenericController().hasAccess(Models.Enums.PermisoEnum.Usuario, Session))
            {
                this.AddNotification("No posees permisos para Eliminar Usuarios.", NotificationType.WARNING);
                return RedirectToAction("Index", "Usuario");
            }
            if (id == null)
            {
                return RedirectToAction("Error", "Home");;
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuario usuario = db.Usuario.Find(id);
            usuario.FechaModificacion = DateTime.Now;
            usuario.Eliminado = true;
            usuario.Estado = Models.Enums.EstadoEnum.Inactivo;
            db.Entry(usuario).State = EntityState.Modified;
            db.SaveChanges();
            this.AddNotification("Usuario eliminado exitosamente", NotificationType.SUCCESS);
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
