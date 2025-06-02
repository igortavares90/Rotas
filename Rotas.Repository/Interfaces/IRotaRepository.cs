using Rotas.Domain.Models;

namespace Rotas.Repository.Interfaces
{
    public interface IRotaRepository
    {
        Task<IEnumerable<Rota>> GetAllAsync();
        Task<Rota?> GetByIdAsync(int id);
        Task<int> AddAsync(Rota rota);
        Task<bool> UpdateAsync(Rota rota);
        Task<bool> DeleteAsync(int id);
    }
}
