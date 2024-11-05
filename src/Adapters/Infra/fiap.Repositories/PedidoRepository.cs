using fiap.Domain.Entities;
using fiap.Domain.Repositories.Interfaces;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace fiap.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly Func<IDbConnection> _connectionFactory;
        private readonly IClienteRepository _clienteRepository;

        public PedidoRepository(Func<IDbConnection> connectionFactory, IClienteRepository clienteRepository )
        {
            _connectionFactory = connectionFactory;
            _clienteRepository = clienteRepository;
        }

        public async Task<List<Pedido>> ObterPedidos()
        {
            using var connection = _connectionFactory();
            connection.Open();
            var lst = new List<Pedido>();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT p.* , sp.Descricao DescricaoStatusPedido FROM Pedido p join StatusPedido sp on p.IdStatusPedido = sp.IdStatusPedido";
            using var reader = command.ExecuteReader();
            while (reader.Read() ) {
                // Map your data to your entity
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
                    Produtos = await ObterItemPedido((int)reader["IdPedido"])
                });
            }
            return await Task.FromResult(lst);
        }

        public async Task<Pedido> ObterPedido(int idPedido)
        {
            using var connection = _connectionFactory();
            connection.Open();
            // Execute your query
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT p.* , sp.Descricao DescricaoStatusPedido FROM Pedido p join StatusPedido sp on p.IdStatusPedido = sp.IdStatusPedido WHERE IdPedido = @id";
            var param = command.CreateParameter();
            param.ParameterName = "@id";
            param.Value = idPedido;
            command.Parameters.Add(param);
            Pedido pedido = null;
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                // Map your data to your entity
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
                    Produtos = await ObterItemPedido(idPedido)
                };
            }
            return pedido;
        }

        private Task<List<Produto>> ObterItemPedido(int idPedido)
        {
            using var connection = _connectionFactory();
            connection.Open();
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
                // Map your data to your entity
                lst.Add(new Produto
                {
                    IdProduto = (int)reader["IdProduto"],
                    IdCategoriaProduto = (int)reader["IdCategoriaProduto"],
                    Nome = reader["Nome"].ToString(),
                    Descricao = reader["Descricao"].ToString(),
                    Preco = (decimal)reader["Preco"]
                    //DataCriacao = (DateTime)reader["DataCriacao"],
                    //DataAlteracao = (DateTime)reader["DataAlteracao"]
                });
            }
            return Task.FromResult(lst);
        }

        public Task<bool> Inserir(Pedido pedido)
        {
            using var connection = _connectionFactory();
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // Execute your query
                    using var command = connection.CreateCommand();
                    command.Transaction = transaction;
                    command.CommandText = "insert into Pedido values(@idCliente, @numeroPedido, @idStatusPedido, @valorTotal, getdate(),null); select cast(@@identity as int)";

                    command.Parameters.Add(new SqlParameter { ParameterName = "@idCliente", Value = pedido.Cliente.Id, SqlDbType = SqlDbType.Int });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@numeroPedido", Value = pedido.Numero, SqlDbType = SqlDbType.VarChar });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@idStatusPedido", Value = pedido.StatusPedido.IdStatusPedido, SqlDbType = SqlDbType.Int });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@valorTotal", Value = pedido.ValorTotal, SqlDbType = SqlDbType.Decimal });

                    var idPedido = (int)command.ExecuteScalar();

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
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Erro: {ex.Message}");
                    return Task.FromResult(false);
                }
            }

            return Task.FromResult(true);

        }

        public Task<bool> Atualizar(Pedido pedido)
        {
            using var connection = _connectionFactory();
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // Execute your query
                    StringBuilder sb = new StringBuilder();
                    using var command = connection.CreateCommand();
                    command.Transaction = transaction;
                    sb.Append("update Pedido set IdStatusPedido = @idStatusPedido,");
                    sb.Append("ValorTotal = @valorTotal, DataAlteracao = getdate() ");
                    sb.Append("where IdPedido = @idPedido");
                    command.CommandText = sb.ToString();

                    command.Parameters.Add(new SqlParameter { ParameterName = "@idPedido", Value = pedido.IdPedido, SqlDbType = SqlDbType.Int });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@idStatusPedido", Value = pedido.StatusPedido.IdStatusPedido, SqlDbType = SqlDbType.Int });

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

                        command2.ExecuteNonQuery();

                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Erro: {ex.Message}");
                    return Task.FromResult(false);
                }
            }

            return Task.FromResult(true);
        }
    }

}
