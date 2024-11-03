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
        // GET:api/<ProdutoController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _produtoApplication.Obter());
        }

        // GET:api/<ProdutoController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _produtoApplication.Obter(id));
        }

        // POST: api/<ProdutoController>/5
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Produto obj)
        {
            if (await _produtoApplication.Inserir(obj))
                return Ok(new { Mensagem = "Incluido com sucesso" });

            return BadRequest(new { Mensagem = "Erro ao incluir" });
        }
        // PUT: api/<ProdutoController>/5
        [HttpPut()]
        public async Task<IActionResult> Put([FromBody] Produto obj)
        {
            if (await _produtoApplication.Atualizar(obj))
                return Ok(new { Mensagem = "Alterado com sucesso" });

            return BadRequest(new { Mensagem = "Erro ao alterar" });
        }

        // GET: api/<ProdutoController>/5
        [HttpGet("idCategoriaProduto/{idCategoriaProduto}")]
        //[Route("api/[controller]/idCategoriaProduto/{idCategoriaProduto}")]
        public async Task<IActionResult> GetProdutosPorCategoria(int idCategoriaProduto)
        {
            return Ok(await _produtoApplication.ObterProdutosPorCategoria(idCategoriaProduto));
        }
    }
}
