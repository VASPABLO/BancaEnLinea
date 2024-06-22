using System.ComponentModel.DataAnnotations;

namespace BancaEnLinea.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [EmailAddress(ErrorMessage = "El campo debe ser un correo electronico valido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string EmailNormalizado { get; set; }
        public string PasswordHash { get; set; }
    }
}
