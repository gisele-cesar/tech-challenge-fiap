using fiap.Application.DTO;
using fiap.Application.Interfaces;
using fiap.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace fiap.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly IPedidoApplication _pedidoApplication;
        private readonly IPagamentoApplication _pagamentoApplication;
        public PedidoController(Serilog.ILogger logger, IPedidoApplication pedidoApplication, IPagamentoApplication pagamentoApplication)
        {
            _logger = logger;
            _pedidoApplication = pedidoApplication;
            _pagamentoApplication = pagamentoApplication;
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
            try
            {
                _logger.Information("Buscando lista de pedidos.");
                return Ok(await _pedidoApplication.ObterPedidos());
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao obter pedidos. Erro: {ex.Message}");
            }
        }

        /// <summary>
        /// Obter Lista de Pedidos Por Status
        /// </summary>
        /// <remarks>   
        /// Descricao Status Pedido para ordenacao:
        /// 
        ///     [
        ///         - Recebido,
        ///         - Em preparação,
        ///         - Pronto
        ///     ]
        /// 
        /// Exemplo:
        /// 
        ///     Get /Pedido
        ///     {
        ///         "status1": "Pronto",
        ///         "status2": "Em preparação",
        ///         "status3": "Recebido"
        ///     }
        ///     
        /// </remarks>
        /// <param name="status1"></param>
        /// <param name="status2"></param>
        /// <param name="status3"></param>
        /// <returns>Retorna lista de Pedidos ordenados por status do pedido conforme parâmetros da consulta</returns>
        /// <response code = "200">Retorna a lista de pedidos ordenados por status do pedido conforme parâmetros da consulta</response>
        /// <response code = "400">Se houver erro na busca por pedidos</response>
        /// <response code = "500">Se houver erro de conexão com banco de dados</response>
        [HttpGet("ObterPedidosPorStatus")]
        public async Task<IActionResult> Get([FromQuery, Required] string status1,
                                             [FromQuery, Required] string status2,
                                             [FromQuery, Required] string status3)
        {
            try
            {
                _logger.Information($"Buscando lista de pedidos por status na ordem: {status1}, {status2} e {status3}");
                return Ok(await _pedidoApplication.ObterPedidosPorStatus(status1, status2, status3));

            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao obter pedidos por status. Erro: {ex.Message}");
            }
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
            try
            {
                _logger.Information($"Buscando pedido por id: {id}.");
                return Ok(await _pedidoApplication.ObterPedido(id));
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao obter pedido por id: {id}. Erro: {ex.Message}");
            }
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
        ///         "listaCodigoProduto": [1]
        ///     }
        /// </remarks>
        /// <param name="pedido"></param>
        /// <returns>Um novo pedido criado que inicia com o status do pedido 'Solicitado' e o status do pagamento 'Pendente'</returns>
        /// <response code = "200">Retorna o novo pedido criado</response>
        /// <response code = "400">Se o pedido não for criado</response>
        /// <response code = "500">Se houver erro de conexão com banco de dados</response>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PedidoDTO pedido)
        {
            try
            {
                var lstProdutos = new List<Produto>();

                foreach (var item in pedido.ListaCodigoProduto)
                    lstProdutos.Add(new Produto { IdProduto = item });

                var obj = new Pedido
                {
                    Cliente = new Cliente { Id = pedido.IdCliente },
                    Numero = pedido.NumeroPedido,
                    Produtos = lstProdutos,
                    StatusPedido = new StatusPedido { IdStatusPedido = 1 },
                    StatusPagamento = new StatusPagamento { IdStatusPagamento = 1 }
                };

                _logger.Information($"Inserindo novo pedido numero: {pedido.NumeroPedido}");
                var pedidoInserido = await _pedidoApplication.Inserir(obj);

                if (pedidoInserido != null)
                {
                    if (await _pagamentoApplication.CriarOrdemPagamento(pedidoInserido.IdPedido))
                    {
                        _logger.Information($"Iniciando criacao ordem de pagamento no MP do pedido id: {pedidoInserido.IdPedido}.");
                        return Ok(new { Mensagem = $"Pedido incluído com sucesso!", pedidoInserido.IdPedido });
                    }
                    return BadRequest(new { Mensagem = "Erro ao criar pedido no meio de pagamento" });
                }
                return BadRequest(new { Mensagem = "Erro ao incluir pedido!" });
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao incluir pedido {pedido.NumeroPedido}. Erro: {ex.Message}");
            }
        }

        /// <summary>
        /// Atualizar pedido
        /// </summary>
        /// <remarks>   
        /// Ids Status Pedido:
        /// 
        ///     [
        ///         1: Solicitado,
        ///         2: Recebido,
        ///         3: Em preparação,
        ///         4: Pronto,
        ///         5: Finalizado
        ///     ]
        ///
        /// Ids Status Pagamento:
        /// 
        ///     [
        ///         1: Pendente,
        ///         2: Aprovado,
        ///         3: Recusado
        ///     ]
        /// 
        /// param listaCodigoProduto:
        /// 
        ///     [idProduto1, idProduto2]
        /// 
        /// Exemplo:
        /// 
        ///     PUT /Pedido
        ///     {
        ///         "idPedido": 1,
        ///         "idCliente": 1,
        ///         "numeroPedido": "1234",
        ///         "idStatusPedido": 2,
        ///         "idStatusPagamento": 2,
        ///         "listaCodigoProduto": [1, 2]
        ///     }
        ///     
        /// </remarks>
        /// <param name="pedido"></param>
        /// <returns>Pedido alterado</returns>
        /// <response code = "200">Retorna se o pedido for alterado</response>
        /// <response code = "400">Se houver erro na alteração do pedido</response>
        /// <response code = "500">Se houver erro de conexão com banco de dados</response>
        [HttpPut()]
        public async Task<IActionResult> Put([FromBody] PedidoDTO pedido)
        {
            try
            {
                var lstProdutos = new List<Produto>();

                foreach (var item in pedido.ListaCodigoProduto)
                    lstProdutos.Add(new Produto { IdProduto = item });
                var obj = new Pedido
                {
                    IdPedido = pedido.IdPedido,
                    StatusPedido = new StatusPedido { IdStatusPedido = pedido.IdStatusPedido },
                    StatusPagamento = new StatusPagamento { IdStatusPagamento = pedido.IdStatusPagamento },
                    Produtos = lstProdutos
                };

                _logger.Information($"Atualizando pedido id: {pedido.IdPedido}.");

                if (await _pedidoApplication.Atualizar(obj))
                    return Ok(new { Mensagem = "Pedido alterado com sucesso!" });

                return BadRequest(new { Mensagem = "Erro ao alterar pedido!" });
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao alterar pedido id {pedido.IdPedido}. Erro: {ex.Message}");
                throw;
            }
        }
    }
}
