using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicalApp.Models.Usuarios
{
    public class AreaGeneral : MainModel
    {
        [MaxLength(100)]
        public string Descripcion { get; set; } = string.Empty;

        public virtual ICollection<AreaEspecialidad> __AreaEspecialidades { get; set; } = null;
    }
}