using fiap.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace fiap.Application.Interfaces
{
    public interface IPedidoApplication
    {
        Task<Pedido> ObterPedido(int idPedido);
        Task<bool> Inserir(Pedido pedido);
    }
}
