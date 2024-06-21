using BancaEnLinea.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BancaEnLinea.Servicios
{
    public interface IRepositorioCliente
    {
        Task CrearAsync(Cliente cliente);
        Task<Cliente> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Cliente>> ObtenerTodosAsync();
        Task ActualizarAsync(Cliente cliente);
        Task EliminarAsync(int id);
    }
    public class RepositorioCliente : IRepositorioCliente
    {
        private readonly string _connectionString;

        public RepositorioCliente(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task CrearAsync(Cliente cliente)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO Cliente (Cedula, Nombre, Apellidos, Correo, Telefono, UsuarioId, Saldo)
                        VALUES (@Cedula, @Nombre, @Apellidos, @Correo, @Telefono, @UsuarioId, @Saldo);
                        SELECT CAST(SCOPE_IDENTITY() as int);";
            var id = await connection.QuerySingleAsync<int>(sql, cliente);
            cliente.Id = id;
        }

        public async Task<Cliente> ObtenerPorIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QuerySingleOrDefaultAsync<Cliente>("SELECT * FROM Cliente WHERE Id = @Id", new { Id = id });
        }

        public async Task<IEnumerable<Cliente>> ObtenerTodosAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<Cliente>("SELECT * FROM Cliente");
        }

        public async Task ActualizarAsync(Cliente cliente)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"UPDATE Cliente SET Cedula = @Cedula, Nombre = @Nombre, Apellidos = @Apellidos, 
                        Correo = @Correo, Telefono = @Telefono, UsuarioId = @UsuarioId, Saldo = @Saldo 
                        WHERE Id = @Id";
            await connection.ExecuteAsync(sql, cliente);
        }

        public async Task EliminarAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync("DELETE FROM Cliente WHERE Id = @Id", new { Id = id });
        }
    }
}




