using fiap.Application.Interfaces;
using fiap.Domain.Entities;
using fiap.Domain.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace fiap.Application.UseCases
{
    public class PagamentoApplication : IPagamentoApplication
    {
        private readonly ILogger _logger;
        private readonly IPedidoApplication _pedidoApplication;
        private readonly IPagamentoExternoService _pagamentoExternoService;

        public PagamentoApplication(ILogger logger, IPedidoApplication pedidoApplication, IPagamentoExternoService pagamentoExternoService)
        {
            _logger = logger;
            _pedidoApplication = pedidoApplication;
            _pagamentoExternoService = pagamentoExternoService;
        }

        public async Task<bool> CriarOrdemPagamento(int idPedido)
        {
            _logger.Information($"Buscando pedido id: {idPedido} para criar ordem de pagamento no MP.");
            var pedido = await _pedidoApplication.ObterPedido(idPedido);

            _logger.Information($"Criando ordem de pagamento no MP para o pedido id: {idPedido}.");
            return await _pagamentoExternoService.CriarOrdemPagamentoExterno(pedido);
        }

        public async Task<object> ConsultarOrdemPagamento(string idOrdemComercial)
        {
            _logger.Information($"Buscando ordem de pagamento no MP do idOrdemComercial: {idOrdemComercial}.");
            return await _pagamentoExternoService.ConsultarOrdemPagamentoExterno(idOrdemComercial);
        }
    }
}
