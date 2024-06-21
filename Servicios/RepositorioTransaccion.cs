using BancaEnLinea.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BancaEnLinea.Servicios
{
    public interface IRepositorioTransaccion
    {
        Task CrearAsync(Transaccion transaccion);
        Task<Transaccion> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Transaccion>> ObtenerTodasAsync();
        Task<IEnumerable<Transaccion>> ObtenerConFiltrosAsync(DateTime? desde, DateTime? hasta, string nombreCliente);
        Task ActualizarAsync(Transaccion transaccion);
        Task EliminarAsync(int id);
        Task EliminarTodosAsync();
        Task AnularAsync(int id);
    }
    public class RepositorioTransaccion : IRepositorioTransaccion
    {
        private readonly string _connectionString;

        public RepositorioTransaccion(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task CrearAsync(Transaccion transaccion)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO Transaccion (ClienteId, Tipo, Monto, Fecha, Anulada)
                        VALUES (@ClienteId, @Tipo, @Monto, @Fecha, @Anulada);
                        SELECT CAST(SCOPE_IDENTITY() as int);";
            var id = await connection.QuerySingleAsync<int>(sql, transaccion);
            transaccion.Id = id;
        }

        public async Task<Transaccion> ObtenerPorIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QuerySingleOrDefaultAsync<Transaccion>("SELECT * FROM Transaccion WHERE Id = @Id", new { Id = id });
        }

        public async Task<IEnumerable<Transaccion>> ObtenerTodasAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<Transaccion>("SELECT * FROM Transaccion WHERE Anulada = 0");
        }

        public async Task<IEnumerable<Transaccion>> ObtenerConFiltrosAsync(DateTime? desde, DateTime? hasta, string nombreCliente)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"SELECT t.* FROM Transaccion t
                        INNER JOIN Cliente c ON t.ClienteId = c.Id
                        WHERE t.Anulada = 0
                        AND (@Desde IS NULL OR t.Fecha >= @Desde)
                        AND (@Hasta IS NULL OR t.Fecha <= @Hasta)
                        AND (@NombreCliente IS NULL OR c.Nombre LIKE '%' + @NombreCliente + '%')";
            return await connection.QueryAsync<Transaccion>(sql, new { Desde = desde, Hasta = hasta, NombreCliente = nombreCliente });
        }

        public async Task ActualizarAsync(Transaccion transaccion)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"UPDATE Transaccion SET ClienteId = @ClienteId, Tipo = @Tipo, Monto = @Monto, Fecha = @Fecha, Anulada = @Anulada WHERE Id = @Id";
            await connection.ExecuteAsync(sql, transaccion);
        }

        public async Task EliminarAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync("DELETE FROM Transaccion WHERE Id = @Id", new { Id = id });
        }

        public async Task EliminarTodosAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync("DELETE FROM Transaccion");
        }

        public async Task AnularAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"UPDATE Transaccion SET Anulada = 1 WHERE Id = @Id";
            await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
