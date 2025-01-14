using fiap.Application.Interfaces;
using fiap.Domain.Entities;
using fiap.Domain.Interfaces;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace fiap.Application.UseCases
{
    public class ClienteApplication : IClienteApplication
    {
        private readonly ILogger _logger;
        private readonly IClienteRepository _clienteRepository;
        public ClienteApplication(ILogger logger, IClienteRepository clienteRepository)
        {
            _logger = logger;
            _clienteRepository = clienteRepository;
        }
        public async Task<bool> Inserir(Cliente cliente)
        {
            return await _clienteRepository.Inserir(cliente);
        }
        public async Task<List<Cliente>> Obter()
        {
            _logger.Information("Buscando lista de clientes");
            return await _clienteRepository.Obter();
        }
        public async Task<Cliente> Obter(int id)
        {

            return await _clienteRepository.Obter(id);
        }
        public async Task<Cliente> ObterPorCpf(string cpf)
        {

            return await _clienteRepository.ObterPorCpf(cpf);
        }
        public async Task<bool> Atualizar(Cliente cliente)
        {
            return await _clienteRepository.Atualizar(cliente);
        }
    }
}
