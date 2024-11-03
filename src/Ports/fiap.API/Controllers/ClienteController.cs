using fiap.Application.Interfaces;
using fiap.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace fiap.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteApplication _clienteApplication;
        public ClienteController(IClienteApplication clienteApplication)
        {
            _clienteApplication = clienteApplication;
        }
        // GET: api/<ClienteController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _clienteApplication.Obter());
        }

        // GET api/<ClienteController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _clienteApplication.Obter(id));
        }
        [HttpGet()]
        [Route("api/[controller]/ObterPorCpf/{cpf?}")]
        public async Task<IActionResult> ObterPorCpf(string cpf)
        {
            return Ok(await _clienteApplication.ObterPorCpf(cpf));
        }
        // POST api/<ClienteController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Cliente cliente)
        {
            if (await _clienteApplication.Inserir(cliente))
                return Ok(new { Mensagem = "Incluido com sucesso" });

            return BadRequest(new { Mensagem = "Erro ao incluir" });
        }

        // PUT api/<ClienteController>/5
        [HttpPut()]
        public async Task<IActionResult> Put([FromBody] Cliente cliente)
        {
            if (await _clienteApplication.Atualizar(cliente))
                return Ok(new { Mensagem = "Alterado com sucesso" });

            return BadRequest(new { Mensagem = "Erro ao alterar" });
        }
    }
}
