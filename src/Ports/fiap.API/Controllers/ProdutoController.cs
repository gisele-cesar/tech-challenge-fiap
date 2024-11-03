using fiap.Application.Interfaces;
using fiap.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace fiap.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoApplication _produtoApplication;
        public ProdutoController(IProdutoApplication produtoApplication)
        {
            _produtoApplication = produtoApplication;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _produtoApplication.Obter());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _produtoApplication.Obter(id));
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Produto obj)
        {
            if (await _produtoApplication.Inserir(obj))
                return Ok(new { Mensagem = "Incluido com sucesso" });

            return BadRequest(new { Mensagem = "Erro ao incluir" });
        }
        [HttpPut()]
        public async Task<IActionResult> Put([FromBody] Produto obj)
        {
            if (await _produtoApplication.Atualizar(obj))
                return Ok(new { Mensagem = "Incluido com sucesso" });

            return BadRequest(new { Mensagem = "Erro ao incluir" });
        }
    }
}
