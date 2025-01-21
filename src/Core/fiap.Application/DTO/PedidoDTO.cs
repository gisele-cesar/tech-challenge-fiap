using System.Collections.Generic;

namespace fiap.Application.DTO
{
    public class PedidoDTO
    {
        public int IdPedido { get; set; }
        public int IdCliente { get; set; }
        public string NumeroPedido { get; set; }
        public int IdStatusPedido { get; set; }
        public int IdStatusPagamento { get; set; }
        public List<int> ListaCodigoProduto { get; set; }
    }
}
