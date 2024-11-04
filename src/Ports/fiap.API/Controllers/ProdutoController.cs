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

        /// <summary>
        /// Buscar produtos
        /// </summary>
        /// <returns>Lista de produtos cadastrados</returns>
        /// <response code = "200">Retorna a lista de produtos cadastrados</response>
        /// <response code = "400">Se houver erro na busca por produtos</response>
        /// <response code = "500">Se houver erro de conexão com banco de dados</response>
        // GET:api/<ProdutoController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _produtoApplication.Obter());
        }

        /// <summary>
        /// Buscar produto por id
        /// </summary>
        /// <param name="id">Id do produto</param>
        /// <returns>Produto por id</returns>
        /// <response code = "200">Retorna a busca do produto por id se existir</response>
        /// <response code = "400">Se o id do produto não existir</response>
        /// <response code = "500">Se houver erro de conexão com banco de dados</response>
        // GET:api/<ProdutoController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _produtoApplication.Obter(id));
        }

        /// <summary>
        /// Criar produto
        /// </summary>
        /// <remarks>
        /// Exemplo:
        /// 
        ///     POST /Produto
        ///     {
        ///         "idCategoriaProduto": 1,
        ///         "nome": "X-Salada da casa",
        ///         "descricao": "Hamburguer com alface e tomate",
        ///         "preco": 15.50
        ///     }
        ///     
        /// Ids Categoria de Produto:
        ///     [
        ///         Lanche = 1,
        ///         Acompanhamento = 2,
        ///         Bebida = 3,
        ///         Sobremesa = 4
        ///     ]
        /// </remarks>
        /// <param name="obj"></param>
        /// <returns>Um novo produto criado</returns>
        /// <response code = "200">Retorna o novo produto criado</response>
        /// <response code = "400">Se o produto não for criado</response>
        /// <response code = "500">Se houver erro de conexão com banco de dados</response>
        // POST: api/<ProdutoController>/5
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Produto obj)
        {
            if (await _produtoApplication.Inserir(obj))
                return Ok(new { Mensagem = "Incluido com sucesso" });

            return BadRequest(new { Mensagem = "Erro ao incluir" });
        }

        /// <summary>
        /// Alterar produto
        /// </summary>
        /// <remarks>
        /// Exemplo:
        /// 
        ///     PUT /Produto
        ///     {
        ///         "idProduto": 1,
        ///         "idCategoriaProduto": 1,
        ///         "nome": "X-Salada da casa",
        ///         "descricao": "Hamburguer com alface e tomate",
        ///         "preco": 21.00,
        ///         "dataCriacao": "2024-11-04T01:23:15.496Z",
        ///         "dataAlteracao": "2024-11-04T01:23:15.496Z"
        ///     }
        ///
        /// Ids Categoria de Produto:
        ///     [
        ///         Lanche = 1,
        ///         Acompanhamento = 2,
        ///         Bebida = 3,
        ///         Sobremesa = 4
        ///     ]
        /// </remarks>
        /// <param name="obj"></param>
        /// <returns>O produto alterado</returns>
        /// <response code = "200">Retorna o produto alterado</response>
        /// <response code = "400">Se houver erro na alteração do produto</response>
        /// <response code = "500">Se houver erro de conexão com banco de dados</response>
        // PUT: api/<ProdutoController>/5
        [HttpPut()]
        public async Task<IActionResult> Put([FromBody] Produto obj)
        {
            if (await _produtoApplication.Atualizar(obj))
                return Ok(new { Mensagem = "Alterado com sucesso" });

            return BadRequest(new { Mensagem = "Erro ao alterar" });
        }

        /// <summary>
        /// Buscar produtos por categoria
        /// </summary>
        /// <remarks>
        /// 
        /// Ids Categoria de Produto:
        ///     [
        ///         Lanche = 1,
        ///         Acompanhamento = 2,
        ///         Bebida = 3,
        ///         Sobremesa = 4
        ///     ]
        /// </remarks>
        /// <param name="idCategoriaProduto">Id da Categoria de produto</param>
        /// <returns>Lista de produtos cadastrados por categoria</returns>
        /// <response code = "200">Retorna a lista de produtos cadastrados por categoria</response>
        /// <response code = "400">Se houver erro na busca de produtos por categoria</response>
        /// <response code = "500">Se houver erro de conexão com banco de dados</response>
        // GET: api/<ProdutoController>/5
        [HttpGet("ObterProdutoPorCategoria/{idCategoriaProduto}")]
        public async Task<IActionResult> GetProdutosPorCategoria(int idCategoriaProduto)
        {
            return Ok(await _produtoApplication.ObterProdutosPorCategoria(idCategoriaProduto));
        }
    }
}
