using fiap.API.Controllers;
using fiap.Application.DTO;
using fiap.Application.Interfaces;
using fiap.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace fiap.Tests.ControllerTests
{
    public class ControllerPedidoTests
    {
        private readonly Pedido pedido;
        private readonly PedidoDTO pedidoDTO;
        private readonly OrdemPagamentoExternoResponse objPagamento;
        private readonly List<Pedido> lstPedido = [];
        public ControllerPedidoTests()
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
            pedidoDTO = new PedidoDTO
            {
                IdCliente = 1,
                IdStatusPagamento = 1,
                ListaCodigoProduto = [1, 2, 3],
                NumeroPedido = "123"

            };

            objPagamento = new OrdemPagamentoExternoResponse
            {
                additional_info = "teste",
                application_id = Guid.NewGuid(),
                cancelled = false,
                client_id = "123",
                collector = new Collector { email = "teste@teste.com", id = 1, nickname = "teste" },
                date_created = DateTime.Now,
                external_reference = "123",
                id = 1,
                is_test = false,
                last_updated = DateTime.Now,
                marketplace = "teste",
                order_status = "teste",
                items = new List<ItemResponse>() { new ItemResponse { category_id = "teste" , currency_id = "123" , description = "teste" , id = "123" , picture_url = "..." ,
             quantity = 1 , title = "teste" , unit_price = 123} },
                notification_url = "...",
                paid_amount = 1,
                payer = new object { },
                payments = new List<object> { },
                payouts = new List<object> { },
                preference_id = "123",
                refunded_amount = 10,
                shipments = new List<object> { },
                shipping_cost = 10,
                site_id = "123",
                sponsor_id = new object { },
                status = "teste",
                total_amount = 10
            };
        }

        [Fact]
        public async Task Get_OKAsync()
        {
            var _application = new Mock<IPedidoApplication>();
            var _pagamentoapplication = new Mock<IPagamentoApplication>();
            var _logger = new Mock<Serilog.ILogger>();
            _application.SetupSequence(x => x.ObterPedidos()).ReturnsAsync(lstPedido);

            PedidoController controller = new(_logger.Object , _application.Object, _pagamentoapplication.Object);
            var result = await controller.Get();

            Assert.NotNull(result);
        }
        [Fact]
        public async Task Get_obterporStatus_OKAsync()
        {
            var _application = new Mock<IPedidoApplication>();
            var _pagamentoapplication = new Mock<IPagamentoApplication>();
            var _logger = new Mock<Serilog.ILogger>();
            _application.SetupSequence(x => x.ObterPedidosPorStatus("","","")).ReturnsAsync(lstPedido);

            PedidoController controller = new(_logger.Object, _application.Object, _pagamentoapplication.Object);
            var result = await controller.Get("","","");

            Assert.NotNull(result);
        }
        [Fact]
        public async Task Get_obterporId_OKAsync()
        {
            var _application = new Mock<IPedidoApplication>();
            var _pagamentoapplication = new Mock<IPagamentoApplication>();
            var _logger = new Mock<Serilog.ILogger>();
            _application.SetupSequence(x => x.ObterPedido(1)).ReturnsAsync(pedido);

            PedidoController controller = new(_logger.Object, _application.Object, _pagamentoapplication.Object);
            var result = await controller.Get(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task POST_OKAsync()
        {
            var _application = new Mock<IPedidoApplication>();
            var _pagamentoapplication = new Mock<IPagamentoApplication>();
            var _logger = new Mock<Serilog.ILogger>();
            _application.SetupSequence(x => x.Inserir(pedido)).ReturnsAsync(pedido);

            PedidoController controller = new(_logger.Object, _application.Object, _pagamentoapplication.Object);
            var result = await controller.Post(pedidoDTO);

            Assert.NotNull(result);
        }
        [Fact]
        public async Task PUT_OKAsync()
        {
            var _application = new Mock<IPedidoApplication>();
            var _pagamentoapplication = new Mock<IPagamentoApplication>();
            var _logger = new Mock<Serilog.ILogger>();
            _application.SetupSequence(x => x.Atualizar(pedido)).ReturnsAsync(true);

            PedidoController controller = new(_logger.Object, _application.Object, _pagamentoapplication.Object);
            var result = await controller.Put(pedidoDTO);

            Assert.NotNull(result);
        }
    }
}
