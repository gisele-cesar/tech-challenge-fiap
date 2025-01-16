using fiap.Domain.Entities;
using fiap.Domain.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace fiap.Services
{
    public class PagamentoExternoService : IPagamentoExternoService
    {
        private const string AccessToken = "TEST-1903899460856100-011422-80dbc9156f2468ca681050b467907060-670480565";
        private const string UserId = "670480565";
        private const string ExternalPosId = "SUC002POS001";
        private readonly IHttpClientFactory _httpClient;

        public PagamentoExternoService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> CriarOrdemPagamentoExterno(Pedido pedido)
        {
            var pagamentoExterno = new OrdemPagamentoExterno
            {
                external_reference = pedido.IdPedido.ToString(),
                notification_url = "https://webhook.site/2557f0a1-e420-483e-b2df-e85322d213c7",
                expiration_date = DateTime.Now.AddMinutes(5).ToString("yyyy-MM-ddTHH:mm:ss.fffzzz"),
                total_amount = pedido.ValorTotal,
                items = pedido.Produtos.Select(produto => new Item
                {
                    sku_number = produto.IdProduto.ToString(),
                    category = produto.IdCategoriaProduto.ToString(),
                    title = produto.Nome,
                    description = produto.Descricao,
                    quantity = 1,
                    unit_measure = "unit",
                    unit_price = produto.Preco,
                    total_amount = produto.Preco
                }).ToList(),
                title = "Pedido de compra",
                description = "Pedido de compra"
            };

            var jsonContent = JsonSerializer.Serialize(pagamentoExterno);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var client = _httpClient.CreateClient("MercadoPago");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);


            var response = await client.PutAsync($"https://api.mercadopago.com/instore/orders/qr/seller/collectors/{UserId}/pos/{ExternalPosId}/qrs", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.Content.ReadAsStringAsync());
                return true;
            }

            Console.WriteLine(response.Content.ReadAsStringAsync());
            return false;
        }

        public async Task<object> ConsultarOrdemPagamentoExterno(string idOrdemComercial)
        {
            var client = _httpClient.CreateClient("MercadoPago");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            var response = await client.GetAsync($"https://api.mercadopago.com/merchant_orders/{idOrdemComercial}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var ordemPagamentoExterno = JsonSerializer.Deserialize<object>(responseContent.ToString());
                Console.WriteLine(ordemPagamentoExterno);
                return ordemPagamentoExterno;
            }

            return null;
        }

    }
}
