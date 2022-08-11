using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalApp.Models.Usuarios
{
    public class UsuarioAsociado : MainModel
    {
        public int DoctorId { get; set; } = 0;
        public int AsistenteId { get; set; } = 0;

        public virtual Usuario _Doctor { get; set; }
        public virtual Usuario _Asistente { get; set; }
    }
}