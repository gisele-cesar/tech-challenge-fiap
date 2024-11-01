using fiap.Domain.Entities;
using System.Threading.Tasks;

namespace fiap.Domain.Repositories.Interfaces
{
    public interface IClienteRepository
    {
        Task<Cliente> ObterPorId(int id);
        Task<bool> Salvar(Cliente cliente);
    }   
}
