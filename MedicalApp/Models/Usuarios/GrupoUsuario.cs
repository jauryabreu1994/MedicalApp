using MedicalApp.Models.Empresas;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicalApp.Models.Usuarios
{
    public class GrupoUsuario : MainModel
    {
        public int EmpresaId { get; set; } = 0;
        [MaxLength(100)]
        public string Descripcion { get; set; } = string.Empty;
        public virtual Empresa _Empresa { get; set; }
        public virtual ICollection<GrupoPermiso> __GrupoPermisos { get; set; } = null;

        public virtual ICollection<Usuario> __Usuarios { get; set; } = null;
    }
}