using fiap.Application.Interfaces;
using fiap.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace fiap.Application.UseCases
{
    public class ClienteApplication : IClienteApplication
    {
        public Task<bool> Salvar(Cliente cliente)
        {
            return Task.FromResult(true);
        }
    }
}
