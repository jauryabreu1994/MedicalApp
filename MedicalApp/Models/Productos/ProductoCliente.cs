
using MedicalApp.Models.Edificios;
using System.ComponentModel.DataAnnotations;

namespace MedicalApp.Models.Productos
{
    public class ProductoCliente : MainModel
    {
        public int HabitacionClienteId { get; set; } = 0;
        public int ProductoId { get; set; } = 0;
        public decimal Cantidad { get; set; } = 0;
        [MaxLength(100)]
        public string Descripcion { get; set; } = string.Empty;


        public virtual Producto _Producto { get; set; }
        public virtual HabitacionCliente _HabitacionCliente { get; set; }
    }
}