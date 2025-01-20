using fiap.Application.Interfaces;
using fiap.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace fiap.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly IProdutoApplication _produtoApplication;
        public ProdutoController(Serilog.ILogger logger, IProdutoApplication produtoApplication)
        {
            _logger = logger;
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
            try
            {
                _logger.Information("Buscando lista de produtos.");
                return Ok(await _produtoApplication.Obter());
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao obter produtos. Erro: {ex.Message}");
            }
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
            try
            {
                _logger.Information($"Buscando produto por id: {id}.");
                return Ok(await _produtoApplication.Obter(id));

            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao obter produto por id: {id}. Erro: {ex.Message}");
            }
        }

        /// <summary>
        /// Criar produto
        /// </summary>
        /// <remarks>
        /// Ids Categoria de Produto:
        /// 
        ///     [
        ///         1: Lanche,
        ///         2: Acompanhamento,
        ///         3: Bebida,
        ///         4: Sobremesa
        ///     ]
        /// 
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
            try
            {
                _logger.Information("Inserindo novo produto.");
                if (await _produtoApplication.Inserir(obj))
                    return Ok(new { Mensagem = "Produto incluido com sucesso!" });

                return BadRequest(new { Mensagem = "Erro ao incluir produto!" });
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao incluir produto. Erro: {ex.Message}");
            }
        }

        /// <summary>
        /// Alterar produto
        /// </summary>
        /// <remarks>
        /// Ids Categoria de Produto:
        /// 
        ///     [
        ///         1: Lanche,
        ///         2: Acompanhamento,
        ///         3: Bebida,
        ///         4: Sobremesa
        ///     ]
        /// 
        /// Exemplo:
        /// 
        ///     PUT /Produto
        ///     {
        ///         "idProduto": 1,
        ///         "idCategoriaProduto": 1,
        ///         "nome": "X-Salada da casa",
        ///         "descricao": "Hamburguer com alface e tomate",
        ///         "preco": 21.00
        ///     }
        ///
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
            try
            {
                _logger.Information($"Alterando produto id: {obj.IdProduto}.");
                if (await _produtoApplication.Atualizar(obj))
                    return Ok(new { Mensagem = "Produto alterado com sucesso!" });

                return BadRequest(new { Mensagem = "Erro ao alterar produto!" });
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao alterar produto. Erro: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Buscar produtos por categoria
        /// </summary>
        /// <remarks>
        /// 
        /// Ids Categoria de Produto:
        /// 
        ///     [
        ///         1: Lanche,
        ///         2: Acompanhamento,
        ///         3: Bebida,
        ///         4: Sobremesa
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
            try
            {
                _logger.Information($"Buscando produtos por categoria idCategoriaProduto: {idCategoriaProduto}.");
                return Ok(await _produtoApplication.ObterProdutosPorCategoria(idCategoriaProduto));

            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao obter produto por categoria id {idCategoriaProduto}. Erro: {ex.Message}");
                throw;
            }
        }
    }
}
