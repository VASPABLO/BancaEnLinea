using System;

namespace BancaEnLinea.Models
{
    public class Transaccion
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string Tipo { get; set; } // "Deposito" o "Retiro"
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public bool Anulada { get; set; }

        // Relación con Cliente
        public Cliente Cliente { get; set; }
    }
}


