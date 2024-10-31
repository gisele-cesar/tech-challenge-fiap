using fiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fiap.Application.Interfaces
{
    public interface IClienteApplication
    {
        Task<bool> Salvar(Cliente cliente);
    }
}
