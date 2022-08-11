
using MedicalApp.Models.Enums;
using MedicalApp.Models.Horarios;

namespace MedicalApp.Models.Usuarios
{
    public class UsuarioHorario : MainModel
    {
        public int UsuarioId { get; set; } = 0;
        public DiasEnum Dia { get; set; } = 0;
        public int HorarioId { get; set; } = 0;
        public int CantidadPacientes { get; set; } = 1;

        public virtual Usuario _Doctor { get; set; }
        public virtual Horario _Horario { get; set; }
    }
}