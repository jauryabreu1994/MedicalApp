

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalApp.Models.Edificios
{
    public class EdificioNivel : MainModel
    {
        public int EdificioId { get; set; } = 0;
        public int Nivel { get; set; } = 1;
        [NotMapped]
        public string Descripcion { get; set; } = string.Empty;
        public virtual Edificio _Edificio { get; set; }
        public virtual ICollection<Habitacion> __Habitaciones { get; set; } = null;
    }
}