
namespace MedicalApp.Models.Clientes
{
    public class ClienteContrasena : MainModel
    {
        public int ClienteId { get; set; } = 0;
        public string Contraseña { get; set; } = string.Empty;

        public virtual Cliente _Cliente { get; set; }
    }
}