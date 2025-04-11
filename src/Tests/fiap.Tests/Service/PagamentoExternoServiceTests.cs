using fiap.Domain.Entities;
using fiap.Domain.Interfaces;
using fiap.Services;
using Moq;
using Moq.Protected;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace fiap.Tests.Service
{
    public class PagamentoExternoServiceTests
    {
        private readonly Pedido pedido;
        public PagamentoExternoServiceTests()
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
        }

        [Fact]
        public async Task CriarOrdemPagamentoExterno_Test()
        {
            var _httpClient = new Mock<IHttpClientFactory>();
            var _secretManagerService = new Mock<ISecretManagerService>();
            var _logger = new Mock<ILogger>();

            var _mockMessageHandler = new Mock<HttpMessageHandler>();

            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", 
                ItExpr.IsAny<HttpRequestMessage>(), 
                ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(
                new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent("{'RequestId': '1' , 'IdTeste':'123'}")
                }
                );
            var client = new HttpClient(_mockMessageHandler.Object);

            _httpClient.Setup(x => x.CreateClient("MercadoPago")).Returns(client);

            _secretManagerService.Setup(_ => _.ObterSecret<SecretMercadoPago>(It.IsAny<string>())).Returns(Task.FromResult(new SecretMercadoPago { AccessToken = "XX", ExternalPosId = "xx", UserId = "xx" }));

            var service = new PagamentoExternoService(_httpClient.Object, _secretManagerService.Object, _logger.Object);
            var result = await service.CriarOrdemPagamentoExterno(pedido);

            Assert.True(result);

        }

        [Fact]
        public async Task ConsultarOrdemPagamentoExterno_Test()
        {
            var _httpClient = new Mock<IHttpClientFactory>();
            var _secretManagerService = new Mock<ISecretManagerService>();
            var _logger = new Mock<ILogger>();

            var _mockMessageHandler = new Mock<HttpMessageHandler>();

            var obj = new OrdemPagamentoExternoResponse { additional_info = "teste", application_id = Guid.NewGuid() , cancelled = false, client_id  = "123", collector = new Collector { email = "teste@teste.com" , id = 1, nickname = "teste" } , date_created = DateTime.Now, external_reference = "123",
             id = 1, is_test = false, last_updated = DateTime.Now, marketplace = "teste" , order_status = "teste",
             items = new List<ItemResponse>() { new ItemResponse { category_id = "teste" , currency_id = "123" , description = "teste" , id = "123" , picture_url = "..." , 
             quantity = 1 , title = "teste" , unit_price = 123} } , notification_url = "..." , paid_amount = 1 , payer = new object { } , payments = new List<object> { } , payouts = new List<object> { } , preference_id = "123" , refunded_amount = 10,
              shipments = new List<object> { } , shipping_cost = 10, site_id = "123" , sponsor_id = new object { } , status = "teste" , total_amount = 10
            };

            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(
                new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(obj) , Encoding.UTF8 , "application/json")
                }
                );
            var client = new HttpClient(_mockMessageHandler.Object);

            _httpClient.Setup(x => x.CreateClient("MercadoPago")).Returns(client);

            _secretManagerService.Setup(_ => _.ObterSecret<SecretMercadoPago>(It.IsAny<string>())).Returns(Task.FromResult(new SecretMercadoPago { AccessToken = "XX", ExternalPosId = "xx", UserId = "xx" }));

            var service = new PagamentoExternoService(_httpClient.Object, _secretManagerService.Object, _logger.Object);
            var result = await service.ConsultarOrdemPagamentoExterno("teste");

            Assert.NotNull(result);

        }
    }
}
