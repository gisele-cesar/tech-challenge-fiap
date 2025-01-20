using fiap.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace fiap.API.Webhooks
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagamentoExternoController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly IPagamentoApplication _pagamentoApplication;
        private readonly IPedidoApplication _pedidoApplication;

        public PagamentoExternoController(Serilog.ILogger logger, IPagamentoApplication pagamentoApplication, IPedidoApplication pedidoApplication)
        {
            _logger = logger;
            _pagamentoApplication = pagamentoApplication;
            _pedidoApplication = pedidoApplication;
        }

        /// <summary>
        /// Receber evento de ordem de pagamento criada
        /// </summary>
        /// <param name="id"></param>
        /// <param name="topic"></param>
        /// <param name="content"></param>
        /// <returns>Retorna ok para confirmação de recepção do evento de ordem criada no meio de pagamento externo</returns>
        /// <response code = "200">Retorna ok para confirmação de recepção do evento</response>
        /// <response code = "400">Se houver erro na busca do pedido para a ordem criada</response>
        /// <response code = "500">Se houver erro de conexão com banco de dados</response>
        [HttpPost("ReceberEventoOrdemCriada")]
        public async Task<IActionResult> ReceberEventoOrdemCriada([FromQuery] string id, [FromQuery] string topic, [FromBody] dynamic content)
        {
            _logger.Information($"Recebendo evento de ordem de pagamento criado no MP. IdOrdemComercial: {id}");
            return Ok(await _pagamentoApplication.ConsultarOrdemPagamento(id));
        }

        /// <summary>
        /// Receber evento de pagamento processado
        /// </summary>
        /// <param name="data_id"></param>
        /// <param name="topic"></param>
        /// <param name="content"></param>
        /// <returns>Retorna ok para confirmação de recepção do evento de pagamento processado no meio de pagamento externo</returns>
        /// <response code = "200">Retorna ok para confirmação de recepção do evento</response>
        /// <response code = "400">Se houver erro na busca por pedidos</response>
        /// <response code = "500">Se houver erro de conexão com banco de dados</response>
        [HttpPost("ReceberEventoPagamentoProcessado")]
        public async Task<IActionResult> ReceberEventoPagamentoProcessado([FromQuery] int data_id, [FromQuery] string topic, [FromBody] dynamic content)
        {
            _logger.Information($"Buscando pedido id: {data_id} para confirmar pagamento processado.");
            var pedido = await _pedidoApplication.ObterPedido(data_id);

            pedido.StatusPagamento.IdStatusPagamento = 2; //Aprovado
            pedido.StatusPedido.IdStatusPedido = 2; // Recebido

            _logger.Information($"Atualizando status pagamento pedido id: {data_id}.");
            await _pedidoApplication.Atualizar(pedido);
            _logger.Information($"Status pagamento pedido id: {data_id} atualizado com sucesso!");

            return Ok();
        }
    }
}
