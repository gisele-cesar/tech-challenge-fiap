using fiap.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace fiap.API.Webhooks
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagamentoExternoController : ControllerBase
    {
        private readonly IPagamentoApplication _pagamentoApplication;
        private readonly IPedidoApplication _pedidoApplication;

        public PagamentoExternoController(IPagamentoApplication pagamentoApplication, IPedidoApplication pedidoApplication)
        {
            _pagamentoApplication = pagamentoApplication;
            _pedidoApplication = pedidoApplication;
        }

        /// <summary>
        /// Receber evento de ordem de pagamento criada no meio de pagamento externo
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
            return Ok(await _pagamentoApplication.ConsultarOrdemPagamento(id));
        }

        /// <summary>
        /// Receber evento de pagamento processado no meio de pagamento externo
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
            var pedido = await _pedidoApplication.ObterPedido(data_id);

            pedido.StatusPagamento.IdStatusPagamento = 2; //Aprovado
            pedido.StatusPedido.IdStatusPedido = 2; // Recebido

            await _pedidoApplication.Atualizar(pedido);

            return Ok();
        }
    }
}
