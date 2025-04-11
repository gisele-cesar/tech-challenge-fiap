using fiap.Domain.Entities;
using fiap.Domain.Interfaces;
using Serilog;
using System.Data;
using System.Text;

namespace fiap.Repositories
{
    public class ItemPedidoRepository : IItemPedidoRepository
    {
        private readonly ILogger _logger;
        private readonly Func<IDbConnection> _connectionFactory;
        private readonly IClienteRepository _clienteRepository;

        public ItemPedidoRepository(ILogger logger, Func<IDbConnection> connectionFactory, IClienteRepository clienteRepository)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
            _clienteRepository = clienteRepository;
        }

        public Task<List<Produto>> ObterItemPedido(int idPedido)
        {
            try
            {
                using var connection = _connectionFactory();
                connection.Open();
                _logger.Information("Conexão com o banco de dados realizada com sucesso!");

                var lst = new List<Produto>();
                using var command = connection.CreateCommand();

                StringBuilder sb = new StringBuilder();
                sb.Append("select * from ItemPedido item ");
                sb.Append("join Produto p on p.IdProduto = item.IdProduto ");
                sb.Append("where IdPedido = @idPedido ");
                var param = command.CreateParameter();
                param.ParameterName = "@idPedido";
                param.Value = idPedido;
                command.Parameters.Add(param);

                command.CommandText = sb.ToString();

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Produto
                    {
                        IdProduto = (int)reader["IdProduto"],
                        IdCategoriaProduto = (int)reader["IdCategoriaProduto"],
                        Nome = reader["Nome"].ToString(),
                        Descricao = reader["Descricao"].ToString(),
                        Preco = (decimal)reader["Preco"]
                    });
                }
                    _logger.Information($"Lista de itens do pedido id: {idPedido} obtida com sucesso!");
                    return Task.FromResult(lst);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erro ao obter itens do pedido {idPedido} . Erro: {ex.Message}.");
                throw;
            }
        }
    }

}
