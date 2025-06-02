using Rotas.Domain.Models;
using Rotas.Repository.Interfaces;
using Rotas.Repository.Repositories;

namespace Rotas.Service.Services
{
    public class RotaService
    {
        private readonly IRotaRepository _rotaRepository;

        public RotaService(IRotaRepository repository)
        {
            _rotaRepository = repository;
        }

        public async Task<IEnumerable<Rota>> GetAllRotasAsync()
        {
            return await _rotaRepository.GetAllAsync();
        }

        public async Task<Rota> GetByIdAsync(int id)
        {
            return await _rotaRepository.GetByIdAsync(id);
        }

        public async Task<int> AddAsync(Rota rota)
        {
            return await _rotaRepository.AddAsync(rota);
        }

        public async Task<bool> UpdateAsync(Rota rota)
        {
            return await _rotaRepository.UpdateAsync(rota);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _rotaRepository.DeleteAsync(id);
        }

        public async Task<(List<string> Caminho, decimal Custo)> CalcularMelhorRotaAsync(string origem, string destino)
        {
            var rotas = await _rotaRepository.GetAllAsync();

            var grafo = new Dictionary<string, List<(string destino, decimal custo)>>();

            foreach (var rota in rotas)
            {
                if (!grafo.ContainsKey(rota.Origem))
                    grafo[rota.Origem] = new List<(string, decimal)>();

                grafo[rota.Origem].Add((rota.Destino, rota.Valor));
            }

            var distancias = new Dictionary<string, decimal>();
            var anteriores = new Dictionary<string, string>();
            var visitados = new HashSet<string>();
            var fila = new PriorityQueue<string, decimal>();

            foreach (var no in grafo.Keys)
                distancias[no] = decimal.MaxValue;

            distancias[origem] = 0;
            fila.Enqueue(origem, 0);

            while (fila.Count > 0)
            {
                var atual = fila.Dequeue();

                if (visitados.Contains(atual))
                    continue;

                visitados.Add(atual);

                if (!grafo.ContainsKey(atual))
                    continue;

                foreach (var vizinho in grafo[atual])
                {
                    var novaDist = distancias[atual] + vizinho.custo;

                    if (novaDist < distancias.GetValueOrDefault(vizinho.destino, decimal.MaxValue))
                    {
                        distancias[vizinho.destino] = novaDist;
                        anteriores[vizinho.destino] = atual;
                        fila.Enqueue(vizinho.destino, novaDist);
                    }
                }
            }

            // Reconstruir o caminho
            var caminho = new List<string>();
            var atualNo = destino;

            // Sem caminho possível
            if (!anteriores.ContainsKey(destino) && origem != destino)
                return (new List<string>(), -1); 

            while (atualNo != null)
            {
                caminho.Insert(0, atualNo);
                anteriores.TryGetValue(atualNo, out atualNo);
            }

            // Origem não alcança destino
            if (caminho.First() != origem)
                return (new List<string>(), -1); 

            return (caminho, distancias[destino]);
        }
    }
}
