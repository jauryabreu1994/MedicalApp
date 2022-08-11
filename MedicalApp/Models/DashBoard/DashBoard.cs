using MedicalApp.Models.Citas;
using MedicalApp.Models.Servicios;
using System.Collections.Generic;

namespace MedicalApp.Models.DashBoard
{
    public class DashBoard
    {
        public int Cant_Consultas { get; set; } = 0;
        public int Cant_VerificacionAnalisis { get; set; } = 0;
        public int Cant_PendientePorConfirmar { get; set; } = 0;
        public int Cant_Citas { get; set; } = 0;
        public string Proxima_Cita { get; set; } = "";
        public List<Cita> Citas { get; set; } = new List<Cita>();
        public List<Servicio> Servicios { get; set; } = new List<Servicio>();
    }
}