using MedicalApp.Models.Empresas;
using MedicalApp.Models.Impuestos;
using MedicalApp.Models.Usuarios;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicalApp.Models.Productos
{
    public class Producto : MainModel
    {
        public int EmpresaId { get; set; } = 0;
        [MaxLength(20)]
        public string ProductoId { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Descripcion { get; set; } = string.Empty;
        [MaxLength(500)]
        public string DescripcionExtendida { get; set; } = string.Empty;
        public decimal Costo { get; set; } = 0;
        public decimal Venta { get; set; } = 0;
        public int ImpuestoId { get; set; } = 0;

        public virtual Empresa _Empresa { get; set; }
        public virtual Impuesto _Impuesto { get; set; }


        public virtual ICollection<ProductoCliente> __ProductoCliente { get; set; }
    }
}