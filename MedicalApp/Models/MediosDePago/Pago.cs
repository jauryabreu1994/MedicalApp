using MedicalApp.Models.Empresas;
using MedicalApp.Models.Enums;
using System.Collections.Generic;

namespace MedicalApp.Models.MediosDePago
{
    public class Pago : MainModel
    {
        public int EmpresaId { get; set; } = 0;
        public string Descripcion { get; set; } = string.Empty;
        public decimal TasaCambio { get; set; } = 1;
        public TipoPagoEnum TipoPago { get; set; } = TipoPagoEnum.Todos;
        public virtual Empresa _Empresa { get; set; }
    }
}