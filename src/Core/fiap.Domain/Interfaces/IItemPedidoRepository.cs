using fiap.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace fiap.Domain.Interfaces
{
    public interface IItemPedidoRepository
    {
        Task<List<Produto>> ObterItemPedido(int idPedido);
    }
}
