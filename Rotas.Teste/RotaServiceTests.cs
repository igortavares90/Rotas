using Moq;
using Rotas.Domain.Models;
using Rotas.Repository.Interfaces;
using Rotas.Service.Services;

namespace Rotas.Teste
{
    public class RotaServiceTests
    {
        [Fact]
        public async Task CalcularMelhorRotaAsync_DeveRetornarVazioSeRotaNaoExistir()
        {
            var mockRepo = new Mock<IRotaRepository>();
            mockRepo.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Rota>
                {
            new Rota { Origem = "GRU", Destino = "BRC", Valor = 10 },
                });

            var service = new RotaService(mockRepo.Object);
            var resultado = await service.CalcularMelhorRotaAsync("GRU", "CDG");

            Assert.Equal(-1, resultado.Custo);
            Assert.Empty(resultado.Caminho);
        }

        [Fact]
        public async Task AdicionarRota_DeveRetornarIdInserido()
        {
            var mockRepo = new Mock<IRotaRepository>();
            var novaRota = new Rota { Origem = "GRU", Destino = "BRC", Valor = 10 };

            mockRepo.Setup(r => r.AddAsync(novaRota)).ReturnsAsync(1);

            var service = new RotaService(mockRepo.Object);
            var resultado = await service.AddAsync(novaRota);

            Assert.Equal(1, resultado);
        }

        [Fact]
        public async Task AtualizarRota_DeveRetornarTrue()
        {
            var mockRepo = new Mock<IRotaRepository>();
            var rota = new Rota { Id = 1, Origem = "GRU", Destino = "CDG", Valor = 100 };

            mockRepo.Setup(r => r.UpdateAsync(rota)).ReturnsAsync(true);

            var service = new RotaService(mockRepo.Object);
            var resultado = await service.UpdateAsync(rota);

            Assert.True(resultado);
        }

        [Fact]
        public async Task ExcluirRota_DeveRetornarTrue()
        {
            var mockRepo = new Mock<IRotaRepository>();

            mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            var service = new RotaService(mockRepo.Object);
            var resultado = await service.DeleteAsync(1);

            Assert.True(resultado);
        }

        [Fact]
        public async Task CalcularMelhorRotaAsync_DeveRetornarRotaMaisBarata()
        {
            var mockRepo = new Mock<IRotaRepository>();

            mockRepo.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Rota>
                {
                    new Rota { Origem = "GRU", Destino = "BRC", Valor = 10 },
                    new Rota { Origem = "BRC", Destino = "SCL", Valor = 5 },
                    new Rota { Origem = "GRU", Destino = "CDG", Valor = 75 },
                    new Rota { Origem = "GRU", Destino = "SCL", Valor = 20 },
                    new Rota { Origem = "GRU", Destino = "ORL", Valor = 56 },
                    new Rota { Origem = "ORL", Destino = "CDG", Valor = 5 },
                    new Rota { Origem = "SCL", Destino = "ORL", Valor = 20 },
                });

            var rotaService = new RotaService(mockRepo.Object);

            var resultado = await rotaService.CalcularMelhorRotaAsync("GRU", "CDG");

            Assert.Equal(40, resultado.Custo);
            Assert.Equal(new List<string> { "GRU", "BRC", "SCL", "ORL", "CDG" }, resultado.Caminho);

        }
    }
}
