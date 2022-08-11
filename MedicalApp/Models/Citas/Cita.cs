using MedicalApp.Models.Clientes;
using MedicalApp.Models.Empresas;
using MedicalApp.Models.Enums;
using MedicalApp.Models.Usuarios;
using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalApp.Models.Citas
{
    public class Cita
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(20)]
        public string CitaId { get; set; } = string.Empty;
        public int EmpresaId { get; set; } = 0;
        public int ClienteId { get; set; } = 0;
        public int AreaEspecialidadId { get; set; } = 0;
        public int UsuarioId { get; set; } = 0;
        public TipoCitaEnum TipoCita { get; set; } = 0;
        public string Comentario { get; set; } = string.Empty;
        public DateTime FechaCita { get; set; } = DateTime.Today;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime FechaModificacion { get; set; } = DateTime.Now;
        public EstadoCitaEnum Estado { get; set; } = EstadoCitaEnum.Pendiente;
        public bool Eliminado { get; set; } = false;


        public virtual Empresa _Empresa { get; set; }
        public virtual Cliente _Cliente { get; set; }
        public virtual Usuario _Usuario { get; set; }
        public virtual AreaEspecialidad _AreaEspecialidad { get; set; }        
    }
}