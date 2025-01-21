using fiap.Domain.Entities;
using fiap.Domain.Interfaces;
using Serilog;
using Serilog.Core;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace fiap.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly ILogger _logger;
        private readonly Func<IDbConnection> _connectionFactory;
        private readonly IClienteRepository _clienteRepository;

        public PedidoRepository(ILogger logger, Func<IDbConnection> connectionFactory, IClienteRepository clienteRepository)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
            _clienteRepository = clienteRepository;
        }

        public async Task<List<Pedido>> ObterPedidos()
        {
            try
            {
                using var connection = _connectionFactory();
                connection.Open();
                _logger.Information("Conexão com o banco de dados realizada com sucesso!");

                var lst = new List<Pedido>();
                using var command = connection.CreateCommand();
                command.CommandText = @"
                                SELECT p.*, sp.Descricao AS DescricaoStatusPedido, pg.Descricao AS DescricaoStatusPagamento 
                                FROM Pedido p 
                                JOIN StatusPedido sp ON p.IdStatusPedido = sp.IdStatusPedido
                                JOIN StatusPagamento pg ON p.IdStatusPagamento = pg.IdStatusPagamento";
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Pedido
                    {
                        IdPedido = (int)reader["IdPedido"],
                        Cliente = await _clienteRepository.Obter((int)reader["IdCliente"]),
                        Numero = reader["NumeroPedido"].ToString(),
                        StatusPedido = new StatusPedido
                        {
                            IdStatusPedido = (int)reader["IdStatusPedido"],
                            Descricao = reader["DescricaoStatusPedido"].ToString()
                        },
                        StatusPagamento = new StatusPagamento
                        {
                            IdStatusPagamento = (int)reader["IdStatusPagamento"],
                            Descricao = reader["DescricaoStatusPagamento"].ToString()
                        },
                        Produtos = await ObterItemPedido((int)reader["IdPedido"])
                    });
                }

                _logger.Information("Lista de pedidos obtida com sucesso!");

                return await Task.FromResult(lst);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erro ao obter pedidos. Erro: {ex.Message}.");
                throw;
            }
        }

        public async Task<List<Pedido>> ObterPedidosPorStatus(string status1, string status2, string status3)
        {
            try
            {
                using var connection = _connectionFactory();
                connection.Open();
                _logger.Information("Conexão com o banco de dados realizada com sucesso!");

                var lst = new List<Pedido>();
                using var command = connection.CreateCommand();
                command.CommandText = $@"
                                  SELECT p.*, sp.Descricao AS DescricaoStatusPedido, pg.Descricao AS DescricaoStatusPagamento 
                                    FROM Pedido p 
                                    JOIN StatusPedido sp ON p.IdStatusPedido = sp.IdStatusPedido
                                    JOIN StatusPagamento pg ON p.IdStatusPagamento = pg.IdStatusPagamento
                                    WHERE sp.Descricao NOT IN ('Solicitado', 'Finalizado')
                                    ORDER BY 
                                        CASE 
                                            WHEN sp.Descricao = '{status1}' THEN 1
                                            WHEN sp.Descricao = '{status2}' THEN 2
                                            WHEN sp.Descricao = '{status3}' THEN 3
                                            ELSE 4
                                        END,
                                        p.DataCriacao ASC";

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Pedido
                    {
                        IdPedido = (int)reader["IdPedido"],
                        Cliente = await _clienteRepository.Obter((int)reader["IdCliente"]),
                        Numero = reader["NumeroPedido"].ToString(),
                        StatusPedido = new StatusPedido
                        {
                            IdStatusPedido = (int)reader["IdStatusPedido"],
                            Descricao = reader["DescricaoStatusPedido"].ToString()
                        },
                        StatusPagamento = new StatusPagamento
                        {
                            IdStatusPagamento = (int)reader["IdStatusPagamento"],
                            Descricao = reader["DescricaoStatusPagamento"].ToString()
                        },
                        Produtos = await ObterItemPedido((int)reader["IdPedido"])
                    });
                }

                _logger.Information("Lista de pedidos ordenada por status com sucesso!");
                return await Task.FromResult(lst);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erro ao obter pedidos por status. Erro: {ex.Message}.");
                throw;
            }

        }

        public async Task<Pedido> ObterPedido(int idPedido)
        {
            try
            {
                using var connection = _connectionFactory();
                connection.Open();
                _logger.Information("Conexão com o banco de dados realizada com sucesso!");

                using var command = connection.CreateCommand();
                command.CommandText = @"
                                SELECT p.*, sp.Descricao AS DescricaoStatusPedido, pg.Descricao AS DescricaoStatusPagamento 
                                FROM Pedido p 
                                JOIN StatusPedido sp ON p.IdStatusPedido = sp.IdStatusPedido
                                JOIN StatusPagamento pg ON p.IdStatusPagamento = pg.IdStatusPagamento
                                WHERE IdPedido = @id";
                var param = command.CreateParameter();
                param.ParameterName = "@id";
                param.Value = idPedido;
                command.Parameters.Add(param);
                Pedido pedido = null;
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    pedido = new Pedido
                    {
                        IdPedido = (int)reader["IdPedido"],
                        Cliente = await _clienteRepository.Obter((int)reader["IdCliente"]),
                        Numero = reader["NumeroPedido"].ToString(),
                        StatusPedido = new StatusPedido
                        {
                            IdStatusPedido = (int)reader["IdStatusPedido"],
                            Descricao = reader["DescricaoStatusPedido"].ToString()
                        },
                        StatusPagamento = new StatusPagamento
                        {
                            IdStatusPagamento = (int)reader["IdStatusPagamento"],
                            Descricao = reader["DescricaoStatusPagamento"].ToString()
                        },

                        Produtos = await ObterItemPedido(idPedido)
                    };
                    _logger.Information($"Pedido id: {idPedido} obtido com sucesso!");
                    return pedido;
                }
                else
                {
                    throw new Exception($"Id pedido {idPedido} não encontrado.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Erro ao obter pedido id {idPedido}. Erro: {ex.Message}.");
                throw;
            }
        }

        private Task<List<Produto>> ObterItemPedido(int idPedido)
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

                if (lst.Count > 0)
                {
                    _logger.Information($"Lista de itens do pedido id: {idPedido} obtida com sucesso!");
                    return Task.FromResult(lst);
                }
                else
                {
                    throw new Exception($"Itens do pedido id: {idPedido} não encontrados.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Erro ao obter itens do pedido {idPedido} . Erro: {ex.Message}.");
                throw;
            }
        }

        public Task<Pedido> Inserir(Pedido pedido)
        {
            using var connection = _connectionFactory();
            connection.Open();
            _logger.Information("Conexão com o banco de dados realizada com sucesso!");

            using (var transaction = connection.BeginTransaction())
            {
                using var command = connection.CreateCommand();
                try
                {
                    command.Transaction = transaction;
                    command.CommandText = "insert into Pedido values(@idCliente, @numeroPedido, @idStatusPedido, @valorTotal, getdate(),null, @idStatusPagamento); select cast(@@identity as int)";

                    command.Parameters.Add(new SqlParameter { ParameterName = "@idCliente", Value = pedido.Cliente.Id, SqlDbType = SqlDbType.Int });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@numeroPedido", Value = pedido.Numero, SqlDbType = SqlDbType.VarChar });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@idStatusPedido", Value = pedido.StatusPedido.IdStatusPedido, SqlDbType = SqlDbType.Int });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@idStatusPagamento", Value = pedido.StatusPagamento.IdStatusPagamento, SqlDbType = SqlDbType.Int });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@valorTotal", Value = pedido.ValorTotal, SqlDbType = SqlDbType.Decimal });

                    var idPedido = (int)command.ExecuteScalar();
                    pedido.IdPedido = idPedido;

                    foreach (var item in pedido.Produtos)
                    {
                        using var command2 = connection.CreateCommand();
                        command2.Transaction = transaction;
                        command2.CommandText = "insert into ItemPedido values(@idPedido, @idProduto)";

                        command2.Parameters.Add(new SqlParameter { ParameterName = "@idPedido", Value = idPedido, SqlDbType = SqlDbType.Int });
                        command2.Parameters.Add(new SqlParameter { ParameterName = "@idProduto", Value = item.IdProduto, SqlDbType = SqlDbType.Int });

                        command2.ExecuteNonQuery();
                    }
                    transaction.Commit();
                    _logger.Information($"Pedido numero {pedido.Numero} inserido com sucesso!. Pedido id: {pedido.IdPedido}.");
                    return Task.FromResult(pedido);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.Error($"Erro ao incluir novo pedido numero {pedido.Numero}. Erro: {ex.Message}.");
                    throw;
                }
            }
        }

        public Task<bool> AtualizarStatusPedido(Pedido pedido)
        {
            using var connection = _connectionFactory();
            connection.Open();
            _logger.Information("Conexão com o banco de dados realizada com sucesso!");
            using (var transaction = connection.BeginTransaction())
            {
                using var command = connection.CreateCommand();
                try
                {
                    StringBuilder sb = new StringBuilder();
                    command.Transaction = transaction;
                    sb.Append("update Pedido set IdStatusPedido = @idStatusPedido, ");
                    sb.Append("ValorTotalPedido = @valorTotalPedido, DataAlteracao = getdate(), ");
                    sb.Append("IdStatusPagamento = @idStatusPagamento ");
                    sb.Append("where IdPedido = @idPedido");
                    command.CommandText = sb.ToString();

                    command.Parameters.Add(new SqlParameter { ParameterName = "@idPedido", Value = pedido.IdPedido, SqlDbType = SqlDbType.Int });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@idStatusPedido", Value = pedido.StatusPedido.IdStatusPedido, SqlDbType = SqlDbType.Int });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@idStatusPagamento", Value = pedido.StatusPagamento.IdStatusPagamento, SqlDbType = SqlDbType.Int });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@valorTotalPedido", Value = pedido.ValorTotal, SqlDbType = SqlDbType.Decimal });

                    command.ExecuteNonQuery();

                    transaction.Commit();
                    _logger.Information($"Status pedido id: {pedido.IdPedido} atualizado com sucesso!");
                    return Task.FromResult(command.ExecuteNonQuery() >= 1);
                }
                catch (Exception ex)
                {
                    _logger.Error($"Erro ao atualizar pedido id: {pedido.IdPedido}. Erro: {ex.Message}");
                    transaction.Rollback();
                    throw;
                }
            }
        }
        public Task<bool> Atualizar(Pedido pedido)
        {
            using var connection = _connectionFactory();
            connection.Open();
            _logger.Information("Conexão com o banco de dados realizada com sucesso!");
            using (var transaction = connection.BeginTransaction())
            {
                using var command = connection.CreateCommand();
                try
                {
                    StringBuilder sb = new StringBuilder();
                    command.Transaction = transaction;
                    sb.Append("update Pedido set IdStatusPedido = @idStatusPedido, ");
                    sb.Append("ValorTotalPedido = @valorTotalPedido, DataAlteracao = getdate(), ");
                    sb.Append("IdStatusPagamento = @idStatusPagamento ");
                    sb.Append("where IdPedido = @idPedido");
                    command.CommandText = sb.ToString();

                    command.Parameters.Add(new SqlParameter { ParameterName = "@idPedido", Value = pedido.IdPedido, SqlDbType = SqlDbType.Int });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@idStatusPedido", Value = pedido.StatusPedido.IdStatusPedido, SqlDbType = SqlDbType.Int });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@idStatusPagamento", Value = pedido.StatusPagamento.IdStatusPagamento, SqlDbType = SqlDbType.Int });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@valorTotalPedido", Value = pedido.ValorTotal, SqlDbType = SqlDbType.Decimal });

                    command.ExecuteNonQuery();

                    using var commandDeletar = connection.CreateCommand();
                    commandDeletar.Transaction = transaction;
                    commandDeletar.CommandText = "delete ItemPedido where idPedido = @idPedido";

                    commandDeletar.Parameters.Add(new SqlParameter { ParameterName = "@idPedido", Value = pedido.IdPedido, SqlDbType = SqlDbType.Int });

                    commandDeletar.ExecuteNonQuery();

                    foreach (var item in pedido.Produtos)
                    {
                        using var command2 = connection.CreateCommand();
                        command2.Transaction = transaction;
                        command2.CommandText = "insert ItemPedido values(@idPedido, @idProduto)";

                        command2.Parameters.Add(new SqlParameter { ParameterName = "@idPedido", Value = pedido.IdPedido, SqlDbType = SqlDbType.Int });
                        command2.Parameters.Add(new SqlParameter { ParameterName = "@idProduto", Value = item.IdProduto, SqlDbType = SqlDbType.Int });

                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                    _logger.Information($"Pedido id: {pedido.IdPedido} atualizado com sucesso!");
                    return Task.FromResult(command.ExecuteNonQuery() >= 1);
                }
                catch (Exception ex)
                {
                    _logger.Error($"Erro ao atualizar pedido id: {pedido.IdPedido}. Erro: {ex.Message}");
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }

}
