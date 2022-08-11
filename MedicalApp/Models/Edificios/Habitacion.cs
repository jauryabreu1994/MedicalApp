
using System.Collections.Generic;

namespace MedicalApp.Models.Edificios
{
    public class Habitacion : MainModel
    {
        public int EdificioNivelId { get; set; } = 1;
        public string CodigoHabitacion { get; set; } = string.Empty;
        public int MaximoClientes { get; set; } = 0;
        public virtual EdificioNivel _EdificioNivel { get; set; }
        public virtual ICollection<HabitacionCliente> __HabitacionClientes { get; set; } = null;
    }
}