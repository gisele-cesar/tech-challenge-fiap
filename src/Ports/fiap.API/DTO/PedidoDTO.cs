using System.Text.Json.Serialization;

namespace fiap.API.DTO
{
    public class PedidoDTO
    {
        [JsonIgnore]
        public int IdPedido { get; set; }
        public int IdCliente { get; set; }
        public string NumeroPedido { get; set; }
        public int IdStatusPedido { get; set; }
        public List<int> ListaCodigoProduto { get; set; }
    }
}
