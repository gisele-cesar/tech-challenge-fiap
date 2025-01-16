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

        [HttpPost("ordemCriada")]
        public async Task<IActionResult> OrdemCriada([FromQuery] string id, [FromQuery] string topic, [FromBody] dynamic content)
        {
            return Ok(await _pagamentoApplication.ConsultarOrdemPagamento(id));
        }


        [HttpPost("pagamentoProcessado")]
        public async Task<IActionResult> PagamentoProcessado([FromQuery] int data_id, [FromQuery] string topic, [FromBody] dynamic content)
        {
            var pedido = await _pedidoApplication.ObterPedido(data_id);

            pedido.StatusPagamento.IdStatusPagamento = 2; //Aprovado
            pedido.StatusPedido.IdStatusPedido = 2; // Recebido

            await _pedidoApplication.Atualizar(pedido);

            return Ok();
        }
    }
}
