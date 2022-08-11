
using MedicalApp.Models.Usuarios;
using System.Collections.Generic;

namespace MedicalApp.Models.Clientes
{
    public class ClienteHistorial : MainModel
    {
        public int ClienteId { get; set; } = 0;
        public int UsuarioId { get; set; } = 0;
        public string Documentacion { get; set; } = string.Empty;
        public string Indicaciones { get; set; } = string.Empty;


        public virtual Cliente _Cliente { get; set; }
        public virtual Usuario _Usuario { get; set; }
        public virtual ICollection<ClienteDocumento> __ClienteDocumentos { get; set; } = null;
    }
}