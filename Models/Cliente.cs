namespace BancaEnLinea.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public int UsuarioId { get; set; }

        // Propiedad Saldo añadida
        public decimal Saldo { get; set; }
    }
}

