using fiap.Application.Interfaces;
using fiap.Domain.Entities;
using fiap.Domain.Interfaces;
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
        private readonly IPedidoApplication _pedidoApplication;
        private readonly IPagamentoExternoService _pagamentoExternoService;

        public PagamentoApplication(IPedidoApplication pedidoApplication, IPagamentoExternoService pagamentoExternoService)
        {
            _pedidoApplication = pedidoApplication;
            _pagamentoExternoService = pagamentoExternoService;
        }

        public async Task<bool> CriarOrdemPagamento(int idPedido)
        {
            var pedido = await _pedidoApplication.ObterPedido(idPedido);

            return await _pagamentoExternoService.CriarOrdemPagamentoExterno(pedido);
        }

        public async Task<object> ConsultarOrdemPagamento(string idOrdemComercial)
        {
            return await _pagamentoExternoService.ConsultarOrdemPagamentoExterno(idOrdemComercial);
            // add log aqui para registrar quando recebermos o response do MP
            // é possível recuperar o external_reference e 
            // utilizar seu retorno para logar:
            // log = "Pedido criado no Meio de Pagamento com sucesso. IdPedido: " + {external_reference};
        }
    }
}
