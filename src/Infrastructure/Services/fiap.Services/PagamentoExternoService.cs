using fiap.Domain.Entities;
using fiap.Domain.Interfaces;
using Serilog;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace fiap.Services
{
    public class PagamentoExternoService : IPagamentoExternoService
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ISecretManagerService _secretManagerService;
        private readonly ILogger _logger;

        public PagamentoExternoService(IHttpClientFactory httpClient, ISecretManagerService secretManagerService, ILogger logger)
        {
            _httpClient = httpClient;
            _secretManagerService = secretManagerService;
            _logger = logger;
        }
        public async Task<bool> CriarOrdemPagamentoExterno(Pedido pedido)
        {
            _logger.Information("Recuperando as valores das secrets dev/fiap/mercado-pago.");
            var secretMercadoPago = await _secretManagerService.ObterSecret<SecretMercadoPago>("dev/fiap/mercado-pago");

            var pagamentoExterno = new OrdemPagamentoExterno
            {
                external_reference = pedido.IdPedido.ToString(),
                notification_url = "http://abf8a0373974c477c988a709a3916975-1818303021.us-east-1.elb.amazonaws.com/api/PagamentoExterno/ReceberEventoOrdemCriada",
                expiration_date = DateTime.Now.AddMinutes(5).ToString("yyyy-MM-ddTHH:mm:ss.fffzzz"),
                total_amount = pedido.ValorTotal,
                items = pedido.Produtos.Select(produto => new Item
                {
                    sku_number = produto.IdProduto.ToString(),
                    category = produto.IdCategoriaProduto.ToString(),
                    title = produto.Nome,
                    description = produto.Descricao,
                    unit_price = produto.Preco,
                    total_amount = produto.Preco
                }).ToList(),
                title = "Pedido de compra",
                description = "Pedido de compra"
            };

            var jsonContent = JsonSerializer.Serialize(pagamentoExterno);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var client = _httpClient.CreateClient("MercadoPago");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", secretMercadoPago.AccessToken);

            var response = await client.PutAsync($"https://api.mercadopago.com/instore/orders/qr/seller/collectors/{secretMercadoPago.UserId}/pos/{secretMercadoPago.ExternalPosId}/qrs", content);

            if (response.IsSuccessStatusCode)
            {
                _logger.Information($"Requisicao de criacao de ordem de pagamento parao pedido id: {pedido.IdPedido} no MP realizada com sucesso! Content: {response.Content.ReadAsStringAsync()}");
                return true;
            }

            _logger.Error($"Erro ao tentar criar ordem de pagamento no MP. Content: {response.Content.ReadAsStream()}");
            return false;
        }

        public async Task<object> ConsultarOrdemPagamentoExterno(string idOrdemComercial)
        {
            _logger.Information("Recuperando as valores das secrets dev/fiap/mercado-pago");

            var secretMercadoPago = await _secretManagerService.ObterSecret<SecretMercadoPago>("dev/fiap/mercado-pago");

            _logger.Information("Realizando chamada do servico 'Orders' do MP.");
            var client = _httpClient.CreateClient("MercadoPago");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", secretMercadoPago.AccessToken);
            var response = await client.GetAsync($"https://api.mercadopago.com/merchant_orders/{idOrdemComercial}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var ordemPagamentoExterno = JsonSerializer.Deserialize<OrdemPagamentoExternoResponse>(responseContent.ToString());
                if (ordemPagamentoExterno != null)
                {
                    _logger.Information($"Ordem de pagamento para o idOrdemComercial: {idOrdemComercial} no MP realizada com sucesso! IdPedido:{ordemPagamentoExterno.external_reference}.");
                    return ordemPagamentoExterno;
                }
            }

            _logger.Error($"Erro ao consultar ordem de pagamento no MP para o idOrdemComercial: {idOrdemComercial}. Content: {response.Content.ReadAsStringAsync()}");
            return null;
        }

    }
}
