using MedicalApp.Models.Citas;
using MedicalApp.Models.Edificios;
using MedicalApp.Models.Impuestos;
using MedicalApp.Models.MediosDePago;
using MedicalApp.Models.Productos;
using MedicalApp.Models.Servicios;
using MedicalApp.Models.Ubicaciones;
using MedicalApp.Models.Usuarios;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicalApp.Models.Empresas
{
    public class Empresa : MainModel
    {
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;
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
        public string Logo { get; set; } = string.Empty;
        public int Moneda { get; set; } = 0;
        public string CodiLicencia { get; set; } = string.Empty;
        public int ServicioId { get; set; } = 1;
        public int ClienteId { get; set; } = 1;
        public int CitaId { get; set; } = 1;
        public int TransaccionId { get; set; } = 1;
        public int CierreId { get; set; } = 1;
        public int DoctorId { get; set; } = 1;
        public int EnfermeraId { get; set; } = 1;
        public int AdministrativoId { get; set; } = 1;
        [MaxLength(100)]
        public string CorreoSMTP { get; set; } = string.Empty;
        [MaxLength(1000)]
        public string ContrasenaSMTP { get; set; } = string.Empty;

        public virtual Pais _Pais { get; set; }
        public virtual Ciudad _Ciudad { get; set; }

        public virtual ICollection<Cita> __Citas { get; set; } = null;
        public virtual ICollection<Usuario> __Usuarios { get; set; } = null;
        public virtual ICollection<GrupoUsuario> __GrupoUsuarios { get; set; } = null;
        public virtual ICollection<Pago> __Pagos { get; set; } = null;
        public virtual ICollection<Edificio> __Edificios { get; set; } = null;
        public virtual ICollection<Producto> __Productos { get; set; } = null;
        public virtual ICollection<Servicio> __Servicios { get; set; } = null;
        public virtual ICollection<Impuesto> __Impuestos { get; set; } = null;

    }
}