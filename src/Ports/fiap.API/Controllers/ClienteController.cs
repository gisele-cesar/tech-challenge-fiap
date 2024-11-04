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

        /// <summary>
        /// Buscar clientes
        /// </summary>
        /// <returns>Lista de clientes cadastrados</returns>
        /// <response code = "200">Retorna a lista de clientes cadastrados</response>
        /// <response code = "400">Se houver erro na busca por clientes</response>
        /// <response code = "500">Se houver erro de conexão com banco de dados</response>
        // GET: api/<ClienteController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _clienteApplication.Obter());
        }

        /// <summary>
        /// Buscar cliente por id
        /// </summary>
        /// <param name="id">Id do cliente</param>
        /// <returns>Cliente por id</returns>
        /// <response code = "200">Retorna a busca do cliente por id se existir</response>
        /// <response code = "400">Se o id do cliente não existir</response>
        /// <response code = "500">Se houver erro de conexão com banco de dados</response>        
        // GET api/<ClienteController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _clienteApplication.Obter(id));
        }

        /// <summary>
        /// Buscar cliente por cpf
        /// </summary>
        /// <param name="cpf">CPF do cliente</param>
        /// <returns>Cliente por cpf</returns>
        /// <response code = "200">Retorna a busca do cliente por cpf se existir</response>
        /// <response code = "400">Se o cpf do cliente não existir</response>
        /// <response code = "500">Se houver erro de conexão com banco de dados</response>  
        [HttpGet("ObterPorCpf/{cpf}")]
        public async Task<IActionResult> ObterPorCpf(string cpf)
        {
            return Ok(await _clienteApplication.ObterPorCpf(cpf));
        }

        /// <summary>
        /// Cadastrar cliente
        /// </summary>
        /// <remarks>
        /// Exemplo:
        /// 
        ///     POST /Cliente
        ///     {
        ///         "Nome": "Maria",
        ///         "cpf": "12345678910",
        ///         "email": "teste@yahoo.com.br"   
        ///     }
        /// 
        /// </remarks>
        /// <param name="cliente"></param>
        /// <returns>Um novo cliente cadastrado</returns>
        /// <response code = "200">Retorna o novo cliente cadastrado</response>
        /// <response code = "400">Se o cliente não for cadastrado</response>
        /// <response code = "500">Se houver erro de conexão com banco de dados</response>        
        // POST api/<ClienteController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Cliente cliente)
        {
            if (await _clienteApplication.Inserir(cliente))
                return Ok(new { Mensagem = "Incluido com sucesso" });

            return BadRequest(new { Mensagem = "Erro ao incluir" });
        }

        /// <summary>
        /// Alterar cadastro cliente
        /// </summary>
        /// <remarks>
        /// Exemplo:
        /// 
        ///     PUT /Cliente
        ///     {
        ///         "idCliente": 1,
        ///         "nome": "Maria Silva",
        ///         "cpf": "12345678910"
        ///         "email": "teste2@yahoo.com.br"      
        ///     }
        ///     
        /// </remarks>
        /// <param name="cliente"></param>
        /// <returns></returns>
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
