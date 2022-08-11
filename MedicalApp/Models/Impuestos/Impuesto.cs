using MedicalApp.Models.Empresas;
using MedicalApp.Models.Productos;
using MedicalApp.Models.Servicios;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicalApp.Models.Impuestos
{
    public class Impuesto : MainModel
    {
        public int EmpresaId { get; set; } = 0;
        [MaxLength(100)]
        public string Descripcion { get; set; } = string.Empty;
        public decimal Monto { get; set; } = 0;
        [MaxLength(10)]
        public string Prefijo { get; set; } = string.Empty;

        public virtual Empresa _Empresa { get; set; }
        public virtual ICollection<Producto> __Productos { get; set; } = null;
        public virtual ICollection<Servicio> __Servicios { get; set; } = null;
    }
}