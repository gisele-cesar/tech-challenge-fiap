using fiap.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace fiap.Application.Interfaces
{
    public interface IPedidoApplication
    {
        Task<List<Pedido>> ObterPedidos();
        Task<List<Pedido>> ObterPedidosPorStatus(string status1, string status2, string status3);
        Task<Pedido> ObterPedido(int idPedido);
        Task<bool> Inserir(Pedido pedido);
        Task<Pedido> InserirPedido(Pedido pedido);
        Task<bool> Atualizar(Pedido pedido);
    }
}
