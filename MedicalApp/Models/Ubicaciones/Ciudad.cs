using MedicalApp.Models.Clientes;
using MedicalApp.Models.Empresas;
using MedicalApp.Models.Usuarios;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicalApp.Models.Ubicaciones
{
    public class Ciudad : MainModel
    {
        public int PaisId { get; set; } = 0;
        [MaxLength(100)]
        public string Descripcion { get; set; } = string.Empty;
        public virtual Pais _Pais { get; set; }

        public virtual ICollection<Empresa> __Empresas { get; set; } = null;
        public virtual ICollection<Cliente> __Clientes { get; set; } = null;
        public virtual ICollection<Usuario> __Usuarios { get; set; } = null;

    }
}