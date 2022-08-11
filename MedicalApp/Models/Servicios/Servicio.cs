using MedicalApp.Models.Empresas;
using MedicalApp.Models.Impuestos;
using MedicalApp.Models.Usuarios;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicalApp.Models.Servicios
{
    public class Servicio : MainModel
    {
        public int EmpresaId { get; set; } = 0;
        [MaxLength(20)]
        public string ServicioId { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Descripcion { get; set; } = string.Empty;
        [MaxLength(500)]
        public string DescripcionExtendida { get; set; } = string.Empty;
        public int UsuarioId { get; set; } = 0;
        public decimal Costo { get; set; } = 0;
        public int ImpuestoId { get; set; } = 0;

        public virtual Empresa _Empresa { get; set; }
        public virtual Usuario _Usuario { get; set; }
        public virtual Impuesto _Impuesto { get; set; }
    }
}