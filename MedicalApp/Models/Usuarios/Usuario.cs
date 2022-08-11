using MedicalApp.Models.Citas;
using MedicalApp.Models.Edificios;
using MedicalApp.Models.Empresas;
using MedicalApp.Models.Enums;
using MedicalApp.Models.Ubicaciones;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicalApp.Models.Usuarios
{
    public class Usuario : MainModel
    {

        public int EmpresaId { get; set; } = 0;
        public int GrupoUsuarioId { get; set; }
        public int AreaEspecialidadId { get; set; }
        [MaxLength(20)]
        public string UsuarioId { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Apellido { get; set; } = string.Empty;
        [MaxLength(20)]
        public string Identificacion { get; set; } = string.Empty;
        public int PaisId { get; set; } = 0;
        public int CiudadId { get; set; } = 0;
        [MaxLength(300)]
        public string Direccion { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Correo { get; set; } = string.Empty;
        [MaxLength(30)]
        public string Telefono { get; set; } = string.Empty;
        public GeneroEnum Genero { get; set; } = GeneroEnum.Masculino;


        public virtual GrupoUsuario _GrupoUsuario { get; set; }
        public virtual AreaEspecialidad _AreaEspecialidad { get; set; }
        public virtual Pais _Pais { get; set; }
        public virtual Ciudad _Ciudad { get; set; }
        public virtual Empresa _Empresa { get; set; }

        public virtual ICollection<UsuarioContrasena> __UsuarioContrasenas { get; set; } = null;
        public virtual ICollection<UsuarioAsociado> __UsuarioDoctorAsociados { get; set; } = null;
        public virtual ICollection<UsuarioAsociado> __UsuarioAsistenteAsociados { get; set; } = null;

        public virtual ICollection<HabitacionCliente> __CajerosHabitaciones { get; set; } = null;
        public virtual ICollection<HabitacionCliente> __DoctoresHabitaciones { get; set; } = null;
        public virtual ICollection<HabitacionCliente> __EnfermerosHabitaciones { get; set; } = null;

        public virtual ICollection<Cita> __Citas { get; set; } = null;

        public virtual ICollection<UsuarioHorario> __UsuariosHorarios { get; set; } = null;
        public virtual ICollection<UsuarioLicencia> __UsuariosLicencias { get; set; } = null;
    }
}