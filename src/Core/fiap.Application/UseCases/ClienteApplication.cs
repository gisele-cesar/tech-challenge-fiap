using fiap.Application.Interfaces;
using fiap.Domain.Entities;
using fiap.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
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
        public async Task<bool> Inserir(Cliente cliente)
        {
            return await _clienteRepository.Inserir(cliente);
        }
        public async Task<List<Cliente>> Obter()
        {

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
