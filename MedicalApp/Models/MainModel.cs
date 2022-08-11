using MedicalApp.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalApp.Models
{
    public class MainModel
    {
        [Key]
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime FechaModificacion { get; set; } = DateTime.Now;
        public EstadoEnum Estado { get; set; } = EstadoEnum.Activo;
        public bool Eliminado { get; set; } = false;
    }
}