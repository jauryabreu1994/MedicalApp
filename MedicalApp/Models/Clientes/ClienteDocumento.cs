using System;

namespace MedicalApp.Models.Clientes
{
    public class ClienteDocumento : MainModel
    {
        public int ClienteHistorialId { get; set; } = 0;
        public Guid Codigo { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public string RutaDocumento { get; set; } = string.Empty;
        public virtual ClienteHistorial _ClienteHistorial { get; set; }
    }
}