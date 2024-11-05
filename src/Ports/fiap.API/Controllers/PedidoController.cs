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

        /// <summary>
        /// Obter Lista de Pedidos
        /// </summary>
        /// <returns>Retorna a lista de pedidos</returns>
        /// <response code = "200">Retorna a lista de pedidos cadastrados</response>
        /// <response code = "400">Se houver erro na busca por pedidos</response>
        /// <response code = "500">Se houver erro de conexão com banco de dados</response>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _pedidoApplication.ObterPedidos());
        }

        /// <summary>
        /// Buscar pedido por id
        /// </summary>
        /// <param name="id">Id do pedido</param>
        /// <returns>Pedido por id</returns>
        /// <response code = "200">Retorna a busca do pedido por id se existir</response>
        /// <response code = "400">Se o id do pedido não existir</response>
        /// <response code = "500">Se houver erro de conexão com banco de dados</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _pedidoApplication.ObterPedido(id));
        }

        /// <summary>
        /// Criar pedido
        /// </summary>
        /// <remarks>
        /// param listaCodigoProduto:
        /// 
        ///     [idProduto1, idProduto2]
        /// 
        /// Exemplo:
        /// 
        ///     POST /Pedido
        ///     {
        ///         "idCliente": 1,
        ///         "numeroPedido": "1234",
        ///         "listaCodigoProduto": [1, 2, 4]
        ///     }
        /// </remarks>
        /// <param name="pedido"></param>
        /// <returns>Um novo pedido criado que inicia com o status 'Recebido'</returns>
        /// <response code = "200">Retorna o novo pedido criado</response>
        /// <response code = "400">Se o pedido não for criado</response>
        /// <response code = "500">Se houver erro de conexão com banco de dados</response>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PedidoDTO pedido)
        {
            var lstProdutos = new List<Produto>();

            foreach (var item in pedido.ListaCodigoProduto)
                lstProdutos.Add(new Produto { IdProduto = item });

            var obj = new Pedido
            {
                Cliente = new Cliente { Id = pedido.IdCliente },
                Numero = pedido.NumeroPedido,
                Produtos = lstProdutos,
                StatusPedido = new StatusPedido { IdStatusPedido = 1 }
            };
            if (await _pedidoApplication.Inserir(obj))
                return Ok(new { Mensagem = "Incluido com sucesso" });

            return BadRequest(new { Mensagem = "Erro ao incluir" });
        }

        /// <summary>
        /// Alterar pedido
        /// </summary>
        /// <remarks>
        /// param listaCodigoProduto:
        /// 
        ///     [idProduto1, idProduto2]
        ///     
        /// Ids Status Pedido:
        /// 
        ///     [
        ///         1: Recebido,
        ///         2: Em preparação,
        ///         3: Pronto,
        ///         4: Finalizado
        ///     ]
        /// 
        /// Exemplo:
        /// 
        ///     PUT /Pedido
        ///     {
        ///         "idPedido": 1,
        ///         "idCliente": 2,
        ///         "numeroPedido": "1234",
        ///         "idStatusPedido": 2,
        ///         "listaCodigoProduto": [1, 2, 4]
        ///     }
        ///     
        /// </remarks>
        /// <param name="pedido"></param>
        /// <returns>Pedido alterado</returns>
        /// <response code = "200">Retorna o pedido alterado</response>
        /// <response code = "400">Se houver erro na alteração do pedido</response>
        /// <response code = "500">Se houver erro de conexão com banco de dados</response>
        [HttpPut()]
        public async Task<IActionResult> Put([FromBody] PedidoDTO pedido)
        {
            var lstProdutos = new List<Produto>();

            foreach (var item in pedido.ListaCodigoProduto)
                lstProdutos.Add(new Produto { IdProduto = item });
            var obj = new Pedido
            {
                IdPedido = pedido.IdPedido,
                Cliente = new Cliente { Id = pedido.IdCliente },
                Numero = pedido.NumeroPedido,
                Produtos = lstProdutos,
                StatusPedido = new StatusPedido { IdStatusPedido = pedido.IdStatusPedido }
            };

            if (await _pedidoApplication.Atualizar(obj))
                return Ok(new { Mensagem = "Alterado com sucesso" });

            return BadRequest(new { Mensagem = "Erro ao alterar" });
        }
    }
}
