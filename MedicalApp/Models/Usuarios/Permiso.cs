using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicalApp.Models.Usuarios
{
    public class Permiso : MainModel
    {
        [MaxLength(100)]
        public string Descripcion { get; set; } = string.Empty;
        public virtual ICollection<GrupoPermiso> __GrupoPermisos { get; set; } = null;
    }
}