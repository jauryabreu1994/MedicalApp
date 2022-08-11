
using System;

namespace MedicalApp.Models.Usuarios
{
    public class UsuarioLicencia : MainModel
    {
        public int UsuarioId { get; set; } = 0;
        public DateTime FechaLicencia { get; set; } = DateTime.Now.Date;

        public virtual Usuario _Usuario { get; set; }
    }
}