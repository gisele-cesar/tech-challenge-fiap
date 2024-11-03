using fiap.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace fiap.Domain.Repositories.Interfaces
{
    public interface IClienteRepository
    {
        Task<Cliente> Obter(int id);
        Task<List<Cliente>> Obter();
        Task<bool> Inserir(Cliente cliente);
        Task<bool> Atualizar(Cliente cliente);
    }   
}
