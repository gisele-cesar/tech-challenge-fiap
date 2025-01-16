using fiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fiap.Application.Interfaces
{
    public interface IPagamentoApplication
    {
        Task<bool> CriarOrdemPagamento(Pedido pedido);
        Task<object> ConsultarOrdemPagamento(int idOrdemComercial);
    }
}
