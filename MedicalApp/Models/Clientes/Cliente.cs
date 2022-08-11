using MedicalApp.Models.Citas;
using MedicalApp.Models.Edificios;
using MedicalApp.Models.Enums;
using MedicalApp.Models.Ubicaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicalApp.Models.Clientes
{
    public class Cliente : MainModel
    {
        [MaxLength(20)]
        public string ClienteId { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Apellido { get; set; } = string.Empty;
        [MaxLength(20)]
        public string Identificacion { get; set; } = string.Empty;
        [MaxLength(100)]
        public string NombreFiscal { get; set; }
        public int PaisId { get; set; } = 0;
        public int CiudadId { get; set; } = 0;
        [MaxLength(300)]
        public string Direccion { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Correo { get; set; } = string.Empty;
        [MaxLength(30)]
        public string Telefono { get; set; } = string.Empty;
        public GeneroEnum Genero { get; set; } = GeneroEnum.Masculino;
        [DataType(DataType.Date), Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaNacimiento { get; set; } = DateTime.Now;

        public virtual Pais _Pais { get; set; }
        public virtual Ciudad _Ciudad { get; set; }
        public virtual ICollection<Cita> __Citas { get; set; } = null;
        public virtual ICollection<HabitacionCliente> __HabitacionClientes { get; set; } = null;
        public virtual ICollection<ClienteHistorial> __ClienteHistoriales { get; set; } = null;
        public virtual ICollection<ClienteContrasena> __ClienteContrasenas { get; set; } = null;

    }
}