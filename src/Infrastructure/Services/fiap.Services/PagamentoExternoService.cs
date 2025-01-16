using fiap.Domain.Entities;
using fiap.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace fiap.Services
{
    public class PagamentoExternoService : IPagamentoExternoService
    {
        private const string AccessToken = "TEST-1903899460856100-011422-80dbc9156f2468ca681050b467907060-670480565";
        private const string UserId = "670480565";
        private const string ExternalPosId = "SUC002POS001";
        private readonly HttpClient _httpClient;

        public PagamentoExternoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> CriarOrdemPagamentoExterno(Pedido pedido)
        {
            // Implementação da chamada da API do serviço de pagamento externo
            // chamar a api externa de pagamento PUT https://api.mercadopago.com/instore/orders/qr/seller/collectors/:userId/pos/:externalPosId/qrs
            // passando os dados do pedido com os params  - {{user_id}} : 670480565 e { { external_pos_id} } : SUC002POS001
            var pagamentoExterno = new OrdemPagamentoExterno
            {
                external_reference = pedido.IdPedido.ToString(),
                notification_url = "https://webhook.site/2557f0a1-e420-483e-b2df-e85322d213c7",
                expiration_date= DateTime.UtcNow.AddMinutes(5),
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
                    total_amount = 1
                }).ToList(),
                title = "Pedido de compra",
                description = "Pedido de compra"
            };

            var jsonContent = JsonSerializer.Serialize(pagamentoExterno);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

            var response = await _httpClient.PutAsync($"https://api.mercadopago.com/instore/orders/qr/seller/collectors/{UserId}/pos/{ExternalPosId}/qrs", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.Content);
                return true;
            }

            Console.WriteLine(response.Content);
            return false;
        }

        public async Task<object> ConsultarOrdemPagamentoExterno(int idOrdemComercial)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            var response = await _httpClient.GetAsync($"https://api.mercadopago.com/merchant_orders/{idOrdemComercial}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var ordemPagamento = JsonSerializer.Deserialize<object>(responseContent);
                Console.WriteLine(ordemPagamento);
                return ordemPagamento;
            }

            return null;
        }

    }
}
