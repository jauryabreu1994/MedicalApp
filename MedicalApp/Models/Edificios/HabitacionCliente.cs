using MedicalApp.Models.Clientes;
using MedicalApp.Models.Productos;
using MedicalApp.Models.Usuarios;
using System.Collections.Generic;

namespace MedicalApp.Models.Edificios
{
    public class HabitacionCliente : MainModel
    {
        public int HabitacionId { get; set; } = 1;
        public int ClienteId { get; set; } = 0;
        public int CajeroId { get; set; } = 0;
        public int DoctorId { get; set; } = 0;
        public int EnfermeraId { get; set; } = 0;


        public virtual Habitacion _Habitacion { get; set; }
        public virtual Cliente _Cliente { get; set; }
        public virtual Usuario _Cajero { get; set; }
        public virtual Usuario _Doctor { get; set; }
        public virtual Usuario _Enfermera { get; set; }

        public virtual ICollection<ProductoCliente> __ProductoCliente { get; set; }
    }
}