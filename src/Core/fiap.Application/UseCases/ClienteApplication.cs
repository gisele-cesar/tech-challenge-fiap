using fiap.Application.Interfaces;
using fiap.Domain.Entities;
using fiap.Domain.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace fiap.Application.UseCases
{
    public class ClienteApplication : IClienteApplication
    {
        private readonly IClienteRepository _clienteRepository;
        public ClienteApplication(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }
        public async Task<bool> Salvar(Cliente cliente)
        {

            return await _clienteRepository.Salvar(cliente);
        }
        public async Task<Cliente> ObterPorId(int id)
        {

            return await _clienteRepository.ObterPorId(id);
        }
    }
}
