
using MedicalApp.Models.Empresas;
using System.Collections.Generic;

namespace MedicalApp.Models.Edificios
{
    public class Edificio : MainModel
    {
        public int EmpresaId { get; set; } = 0;
        public string Descripcion { get; set; } = string.Empty;

        public virtual Empresa _Empresa { get; set; }
        public virtual ICollection<EdificioNivel> __EdificioNiveles { get; set; } = null;
    }
}