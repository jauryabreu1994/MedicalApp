using MedicalApp.Models.Clientes;
using MedicalApp.Models.Empresas;
using MedicalApp.Models.Usuarios;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicalApp.Models.Ubicaciones
{
    public class Pais : MainModel
    {
        [MaxLength(10)]
        public string CodigoArea { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Descripcion { get; set; } = string.Empty;
        [MaxLength(10)]
        public string CodigoTelefono { get; set; } = string.Empty;
        public virtual ICollection<Ciudad> __Ciudades { get; set; } = null;
        public virtual ICollection<Empresa> __Empresas { get; set; } = null;
        public virtual ICollection<Cliente> __Clientes { get; set; } = null;
        public virtual ICollection<Usuario> __Usuarios { get; set; } = null;
    }
}