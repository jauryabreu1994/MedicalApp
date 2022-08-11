using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicalApp.Models.Usuarios
{
    public class AreaEspecialidad : MainModel
    {
        public int AreaGeneralId { get; set; } = 0;
        [MaxLength(100)]
        public string Descripcion { get; set; } = string.Empty;
        public virtual AreaGeneral _AreaGeneral { get; set; }

        public virtual ICollection<Usuario> __Usuarios { get; set; } = null;

    }
}