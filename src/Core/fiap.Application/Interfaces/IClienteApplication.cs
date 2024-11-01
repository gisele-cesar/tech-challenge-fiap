using fiap.Domain.Entities;
using System.Threading.Tasks;

namespace fiap.Application.Interfaces
{
    public interface IClienteApplication
    {
        Task<bool> Salvar(Cliente cliente);
        Task<Cliente> ObterPorId(int id);
    }
}
