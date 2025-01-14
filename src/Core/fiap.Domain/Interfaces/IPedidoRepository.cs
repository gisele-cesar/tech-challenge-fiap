using fiap.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace fiap.Domain.Interfaces
{
    public interface IPedidoRepository
    {
        Task<List<Pedido>> ObterPedidos();
        Task<Pedido> ObterPedido(int idPedido);
        Task<bool> Inserir(Pedido pedido);
        Task<bool> Atualizar(Pedido pedido);
    }
}
