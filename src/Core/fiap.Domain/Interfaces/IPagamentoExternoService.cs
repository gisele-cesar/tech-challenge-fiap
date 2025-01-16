using fiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fiap.Domain.Interfaces
{
    public interface IPagamentoExternoService
    {
        Task<bool> CriarOrdemPagamentoExterno(Pedido pedido);
        Task<object> ConsultarOrdemPagamentoExterno(string idOrdemComercial);
    }
}
