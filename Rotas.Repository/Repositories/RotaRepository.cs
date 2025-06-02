using Dapper;
using Rotas.Domain.Models;
using Rotas.Repository.Interfaces;
using System.Data;

namespace Rotas.Repository.Repositories
{
    public class RotaRepository: IRotaRepository
    {
        private readonly IDbConnection _connection;

        public RotaRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<Rota>> GetAllAsync()
        {
            var sql = "SELECT * FROM Rotas";
            return await _connection.QueryAsync<Rota>(sql);
        }

        public async Task<Rota> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Rotas WHERE Id = @Id";
            return (await _connection.QueryAsync<Rota>(sql, new { Id = id })).FirstOrDefault();
        }

        public async Task<int> AddAsync(Rota rota)
        {
            var sql = @"INSERT INTO Rotas (Origem, Destino, Valor) 
                        VALUES (@Origem, @Destino, @Valor);
                        SELECT CAST(SCOPE_IDENTITY() as int);";
            return await _connection.ExecuteScalarAsync<int>(sql, rota);
        }

        public async Task<bool> UpdateAsync(Rota rota)
        {
            var sql = @"UPDATE Rotas 
                        SET Origem = @Origem, Destino = @Destino, Valor = @Valor 
                        WHERE Id = @Id";
            var rows = await _connection.ExecuteAsync(sql, rota);
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Rotas WHERE Id = @Id";
            var rows = await _connection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}
