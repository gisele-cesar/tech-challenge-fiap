using fiap.API.DTO;
using fiap.Application.Interfaces;
using fiap.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace fiap.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoApplication _pedidoApplication;
        public PedidoController(IPedidoApplication pedidoApplication)
        {
            _pedidoApplication = pedidoApplication;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _pedidoApplication.ObterPedido(id));
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PedidoDTO pedido)
        {
            var lstProdutos = new List<Produto>();

            foreach (var item in pedido.ListaCodigoProduto) 
                lstProdutos.Add(new Produto { IdProduto = item });
           
            var obj = new Pedido { Cliente = new Cliente { Id = pedido.IdCliente }
            , Numero = pedido.NumeroPedido,
             Produtos = lstProdutos,
             StatusPedido = new StatusPedido { IdStatusPedido = 1 }
            };
            if (await _pedidoApplication.Inserir(obj))
                return Ok(new { Mensagem = "Incluido com sucesso" });

            return BadRequest(new { Mensagem = "Erro ao incluir" });
        }
        //[HttpPut()]
        //public async Task<IActionResult> Put([FromBody] Produto obj)
        //{
        //    if (await _produtoApplication.Atualizar(obj))
        //        return Ok(new { Mensagem = "Alterado com sucesso" });

        //    return BadRequest(new { Mensagem = "Erro ao alterar" });
        //}
    }
}
