using BancaEnLinea.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BancaEnLinea.Servicios
{
    public interface IRepositorioUsuarios
    {
        Task<Usuario> BuscarUsuarioPorEimal(string emailNormalizado);
        Task<int> CrearUsuario(Usuario usuario);
    }
    public class RepositorioUsuarios : IRepositorioUsuarios
    {
        private readonly string _connectionString;
        public RepositorioUsuarios(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> CrearUsuario(Usuario usuario)
        {
         
            using var connection = new SqlConnection(_connectionString);
            var id = await connection.QuerySingleAsync<int>(@"
                    INSERT INTO Usuarios (Email,EmailNormalizado, PasswordHash)
                    VALUES (@Email, @EmailNormalizado, @PasswordHash);
                    SELECT SCOPE_IDENTITY();
                    ", usuario);

            return id;
        }

        public async Task<Usuario> BuscarUsuarioPorEimal(string emailNormalizado)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QuerySingleOrDefaultAsync<Usuario>(
                "SELECT * FROM Usuarios where EmailNormalizado = @emailNormalizado", new { emailNormalizado });
        }
    }  
}
