using fiap.API.Controllers;
using fiap.Application.Interfaces;
using fiap.Application.UseCases;
using fiap.Domain.Entities;
using fiap.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fiap.Tests.Application
{
    public class PedidoApplicationTests
    {
        private readonly Pedido pedido;
        private readonly List<Pedido> lstPedido = [];
        public PedidoApplicationTests()
        {
            pedido = new Pedido
            {
                Cliente = new Cliente { Cpf = "12345678910", Email = "teste@teste.com", Id = 1, Nome = "Joao da Silva" },
                IdPedido = 1,
                Numero = "123456",
                StatusPagamento = new StatusPagamento { Descricao = "xx", IdStatusPagamento = 1 }
                 ,
                StatusPedido = new StatusPedido { IdStatusPedido = 1, Descricao = "xx" }
                 ,
                Produtos = [
                     new() { Preco = 1 , Nome = "teste" , IdProduto = 1, IdCategoriaProduto = 1 , Descricao = "teste" , DataCriacao = DateTime.Now , DataAlteracao = DateTime.Now },
                      new() { Preco = 2 , Nome = "teste2" , IdProduto = 2, IdCategoriaProduto = 2 , Descricao = "teste2" , DataCriacao = DateTime.Now , DataAlteracao = DateTime.Now }
                 ]
            };
            lstPedido = [
                pedido,
                pedido
            ];
        }
        [Fact]
        public async Task ObterPedidoPorId_OkAsync()
        {
            var _repo = new Mock<IPedidoRepository>();
            var _logger = new Mock<Serilog.ILogger>();

            _repo.SetupSequence(x => x.ObterPedido(1))
                .ReturnsAsync(pedido);

            PedidoApplication app = new(_logger.Object, _repo.Object);
            var result = await app.ObterPedido(1);

            Assert.NotNull(result);
        }
        [Fact]
        public async Task Obter_OkAsync()
        {
            var _repo = new Mock<IPedidoRepository>();
            var _logger = new Mock<Serilog.ILogger>();

            _repo.SetupSequence(x => x.ObterPedidos())
                .ReturnsAsync(lstPedido);

            PedidoApplication app = new(_logger.Object, _repo.Object);
            var result = await app.ObterPedidos();

            Assert.NotNull(result);
        }
        [Fact]
        public async Task ObterPedidosPorStatus_OkAsync()
        {
            var _repo = new Mock<IPedidoRepository>();
            var _logger = new Mock<Serilog.ILogger>();

            _repo.SetupSequence(x => x.ObterPedidosPorStatus("1" , "2" , "3"))
                .ReturnsAsync(lstPedido);

            PedidoApplication app = new(_logger.Object, _repo.Object);
            var result = await app.ObterPedidosPorStatus("1", "2", "3");

            Assert.NotNull(result);
        }
        [Fact]
        public async Task Inserir_OkAsync()
        {
            var _repo = new Mock<IPedidoRepository>();
            var _logger = new Mock<Serilog.ILogger>();

            _repo.SetupSequence(x => x.Inserir(pedido))
                .ReturnsAsync(pedido);

            PedidoApplication app = new(_logger.Object, _repo.Object);
            var result = await app.Inserir(pedido);

            Assert.NotNull(result);
        }
        [Fact]
        public async Task Atualizar_OkAsync()
        {
            var _repo = new Mock<IPedidoRepository>();
            var _logger = new Mock<Serilog.ILogger>();

            _repo.SetupSequence(x => x.Atualizar(pedido))
                .ReturnsAsync(true);

            PedidoApplication app = new(_logger.Object, _repo.Object);
            var result = await app.Atualizar(pedido);

            Assert.True(result);
        }
    }
}
