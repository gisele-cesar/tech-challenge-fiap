using fiap.Application.Interfaces;
using fiap.Domain.Entities;
using fiap.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<bool> CriarOrdemPagamento(Pedido pedido)
        {
            return await _pagamentoExternoService.CriarOrdemPagamentoExterno(pedido);
        }

        public async Task<object> ConsultarOrdemPagamento(int idOrdemComercial)
        {
            // realiza a chamada do médoto de consulta da API externa e retorna o objeto do pedido criado no MP
            // implementação do método - chamada API externa para consultar a ordem no MP
            return await _pagamentoExternoService.ConsultarOrdemPagamentoExterno(idOrdemComercial);
            // é possível recuperar o external_reference e 
            // utilizar seu retorno para logar:
            // log = "Pedido criado no Meio de Pagamento com sucesso. IdPedido: " + {external_reference};
        }
    }
}
