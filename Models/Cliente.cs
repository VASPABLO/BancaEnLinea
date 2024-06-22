using System.ComponentModel.DataAnnotations;

namespace BancaEnLinea.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(9, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres")]
        [RegularExpression(@"^\d{1,9}$", ErrorMessage = "El campo {0} debe contener solo números")]
        public string Cedula { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Apellidos { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [EmailAddress(ErrorMessage = "El campo {0} no es una dirección de correo electrónico válida")]
        public string Correo { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(8, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener entre {2} y {1} dígitos")]
        [RegularExpression(@"^\d{1,8}$", ErrorMessage = "El campo {0} debe contener solo números")]
        public string Telefono { get; set; }

        public int UsuarioId { get; set; }

        // Propiedad Saldo añadida
        public decimal Saldo { get; set; }
    }
}

