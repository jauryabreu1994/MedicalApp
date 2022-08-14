using MedicalApp.Context;
using MedicalApp.Models.Enums;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Web;
using System.Text.RegularExpressions;

namespace MedicalApp.Controllers
{
    public class GenericController :  Controller
    {
        public string SetFormatVatNumber(string VatNumber)
        {
            VatNumber = VatNumber.Replace(" ", "");
            if (new String(VatNumber.Where(Char.IsLetter).ToArray()).Length == 0)
            {
                if (VatNumber.Replace(" ", "").Length == 9)
                {
                    VatNumber = Convert.ToInt64(new String(VatNumber.Where(Char.IsDigit).ToArray())).ToString("###-#####-#");
                }
                else if (VatNumber.Length == 11)
                {
                    VatNumber = Convert.ToInt64(new String(VatNumber.Where(Char.IsDigit).ToArray())).ToString("###-#######-#");
                }
            }

            return VatNumber;
        }

        public string SetFormatPhoneNumer(string Phone)
        {
            Regex reg = new Regex("[-*'\"(),_&#^@]");
            Phone = reg.Replace(Phone.Replace(" ", string.Empty), string.Empty);

            if (new String(Phone.Where(Char.IsLetter).ToArray()).Length == 0)
            {
                if (Phone.Length == 10)
                {
                    Phone = Convert.ToInt64(new String(Phone.Where(Char.IsDigit).ToArray())).ToString("(###) ### ####");
                }
                else if (Phone.Length == 11)
                {
                    Phone = Convert.ToInt64(new String(Phone.Where(Char.IsDigit).ToArray())).ToString("+# (###) ### ####");
                }
            }

            return Phone;
        }

        public bool IdentificationExist(string id, bool empleado = true) 
        {
            if (!string.IsNullOrEmpty(id)) 
            {
                Regex reg = new Regex("[-*'\"(),_&#^@]");
                id = reg.Replace(id, string.Empty);

                id = SetFormatVatNumber(id);

                MainDbContext db = new MainDbContext();
                if (!empleado)
                {
                    if (db.Cliente.Where(a => a.Identificacion == id).Count() > 0)
                        return true;
                }
                else 
                {
                    if (db.Usuario.Where(a => a.Identificacion == id).Count() > 0)
                        return true;
                }

            }
            return false;
        }

        public bool hasAccess(PermisoEnum permiso, HttpSessionStateBase session, bool isUser = true) 
        {
            try
            {
                MainDbContext db = new MainDbContext();
                Encriptar_DesEncriptar encriptar_DesEncriptar = new Encriptar_DesEncriptar();

                if (session["UserID"] != null)
                {
                    if (isUser)
                    {
                        int UserID = Convert.ToInt32(encriptar_DesEncriptar.DesEncriptar(session["UserID"].ToString()));
                        int GrupoUsuarioId = Convert.ToInt32(encriptar_DesEncriptar.DesEncriptar(session["GrupoUsuarioId"].ToString()));

                        var grupoPermisos = db.GrupoPermiso.Include(e => e._GrupoUsuario).Include(e => e._Permiso).Where(a => a.GrupoUsuarioId == GrupoUsuarioId).ToList();

                        return grupoPermisos.Where(a => a.PermisoId == (int)permiso).Count() == 0 ? false : true;
                    }
                    else {
                        int UserID = Convert.ToInt32(encriptar_DesEncriptar.DesEncriptar(session["UserID"].ToString()));
                        int GrupoUsuarioId = Convert.ToInt32(encriptar_DesEncriptar.DesEncriptar(session["GrupoUsuarioId"].ToString()));

                        return GrupoUsuarioId != 99 ? false : true;
                    }
                }
                return false;
            }
            catch (Exception) 
            {
                return false;
            }
        }
    }
}