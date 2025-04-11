using fiap.Domain.Entities;
using fiap.Domain.Interfaces;
using Serilog;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace fiap.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly ILogger _logger;
        private readonly Func<IDbConnection> _connectionFactory;

        public ProdutoRepository(ILogger logger, Func<IDbConnection> connectionFactory)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
        }

        public Task<Produto> Obter(int id)
        {
            try
            {
                using var connection = _connectionFactory();
                connection.Open();
                _logger.Information("Conexão com o banco de dados realizada com sucesso!");

                using var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Produto WHERE IdProduto = @id";
                var param = command.CreateParameter();
                param.ParameterName = "@id";
                param.Value = id;
                command.Parameters.Add(param);

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    _logger.Information($"Produto id: {id} obtido com sucesso!");
                    var produto = new Produto
                    {
                        IdProduto = (int)reader["IdProduto"],
                        IdCategoriaProduto = (int)reader["IdCategoriaProduto"],
                        Nome = reader["Nome"].ToString(),
                        Descricao = reader["Descricao"].ToString(),
                        Preco = (decimal)reader["Preco"],
                        DataCriacao = (DateTime)reader["DataCriacao"],
                        DataAlteracao = reader["DataAlteracao"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["DataAlteracao"]
                    };

                    return Task.FromResult(produto);
                }
                else
                {
                    throw new Exception($"Id produto {id} não encontrado.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Erro ao obter produto id: {id}. Erro: {ex.Message}.");
                throw;
            }
        }

        public Task<bool> Inserir(Produto Produto)
        {
            try
            {
            using var connection = _connectionFactory();
            connection.Open();
            _logger.Information("Conexão com o banco de dados realizada com sucesso!");

            using var command = connection.CreateCommand();
                command.CommandText = "insert into Produto values (@idCategoriaProduto, @nome, @descricao, @preco, @dataCriacao, @dataAlteracao)";

                command.Parameters.Add(new SqlParameter { ParameterName = "@idCategoriaProduto", Value = Produto.IdCategoriaProduto, SqlDbType = SqlDbType.Int });
                command.Parameters.Add(new SqlParameter { ParameterName = "@nome", Value = Produto.Nome, SqlDbType = SqlDbType.VarChar });
                command.Parameters.Add(new SqlParameter { ParameterName = "@descricao", Value = Produto.Descricao, SqlDbType = SqlDbType.VarChar });
                command.Parameters.Add(new SqlParameter { ParameterName = "@preco", Value = Produto.Preco, SqlDbType = SqlDbType.Decimal });
                command.Parameters.Add(new SqlParameter { ParameterName = "@dataCriacao", Value = DateTime.Now, SqlDbType = SqlDbType.DateTime });
                command.Parameters.Add(new SqlParameter { ParameterName = "@dataAlteracao", Value = System.DBNull.Value, SqlDbType = SqlDbType.DateTime });

                _logger.Information($"Novo produto inserido com sucesso!");

                return Task.FromResult(command.ExecuteNonQuery() >= 1);

            }
            catch (Exception ex)
            {
                _logger.Error($"Erro ao incluir produto. Erro: {ex.Message}");
                return Task.FromResult(false);
            }

        }
        public Task<bool> Atualizar(Produto Produto)
        {
            try
            {
            using var connection = _connectionFactory();
            StringBuilder sb = new StringBuilder();
            connection.Open();
            _logger.Information("Conexão com o banco de dados realizada com sucesso!");

            using var command = connection.CreateCommand();
                sb.Append("update Produto set Nome = @nome,");
                sb.Append("Descricao = @descricao, Preco = @preco, DataAlteracao = @dataAlteracao");
                sb.Append(" where IdProduto = @idProduto");

                command.CommandText = sb.ToString();

                command.Parameters.Add(new SqlParameter { ParameterName = "@idProduto", Value = Produto.IdProduto, SqlDbType = SqlDbType.Int });
                command.Parameters.Add(new SqlParameter { ParameterName = "@idCategoriaProduto", Value = Produto.IdCategoriaProduto, SqlDbType = SqlDbType.Int });
                command.Parameters.Add(new SqlParameter { ParameterName = "@nome", Value = Produto.Nome, SqlDbType = SqlDbType.VarChar });
                command.Parameters.Add(new SqlParameter { ParameterName = "@descricao", Value = Produto.Descricao, SqlDbType = SqlDbType.VarChar });
                command.Parameters.Add(new SqlParameter { ParameterName = "@preco", Value = Produto.Preco, SqlDbType = SqlDbType.Decimal });
                command.Parameters.Add(new SqlParameter { ParameterName = "@dataAlteracao", Value = DateTime.Now, SqlDbType = SqlDbType.DateTime });

                _logger.Information($"Produto id {Produto.IdProduto} atualizado com sucesso!");

                return Task.FromResult(command.ExecuteNonQuery() >= 1);

            }
            catch (Exception ex)
            {
                _logger.Error($"Erro ao atualizar produto id {Produto.IdProduto}. Erro: {ex.Message}.");
                return Task.FromResult(false);
            }

        }
        public Task<List<Produto>> Obter()
        {
            using var connection = _connectionFactory();
            connection.Open();
            _logger.Information("Conexão com o banco de dados realizada com sucesso!");

            var lst = new List<Produto>();
            using var command = connection.CreateCommand();
            try
            {
                command.CommandText = "SELECT * FROM Produto";

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Produto
                    {
                        IdProduto = (int)reader["IdProduto"],
                        IdCategoriaProduto = (int)reader["IdCategoriaProduto"],
                        Nome = reader["Nome"].ToString(),
                        Descricao = reader["Descricao"].ToString(),
                        Preco = (decimal)reader["Preco"],
                        DataCriacao = (DateTime)reader["DataCriacao"],
                        DataAlteracao = reader["DataAlteracao"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["DataAlteracao"]
                    });
                }

                _logger.Information("Lista de produtos obtida com sucesso!");

                return Task.FromResult(lst);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erro ao obter lista de produtos. Erro: {ex.Message}.");
                throw;
            }
        }

        public Task<List<Produto>> ObterProdutosPorCategoria(int idCategoriaProduto)
        {
            try
            {
            using var connection = _connectionFactory();
            connection.Open();
            _logger.Information("Conexão com o banco de dados realizada com sucesso!");

            var lst = new List<Produto>();
            using var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Produto WHERE IdCategoriaProduto = @idCategoriaProduto";
                var param = command.CreateParameter();
                param.ParameterName = "@idCategoriaProduto";
                param.Value = idCategoriaProduto;
                command.Parameters.Add(param);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Produto
                    {
                        IdProduto = (int)reader["IdProduto"],
                        IdCategoriaProduto = (int)reader["IdCategoriaProduto"],
                        Nome = reader["Nome"].ToString(),
                        Descricao = reader["Descricao"].ToString(),
                        Preco = (decimal)reader["Preco"],
                        DataCriacao = (DateTime)reader["DataCriacao"],
                        DataAlteracao = reader["DataAlteracao"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["DataAlteracao"]
                    });
                }

                if (lst.Count > 0)
                {
                _logger.Information($"Lista de produtos de idCategoriaProduto: {idCategoriaProduto} obtida com sucesso!");
                return Task.FromResult(lst);
                }
                else
                {
                    throw new Exception($"Nenhum produto encontrado para idCategoriaProduto: {idCategoriaProduto}.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Erro ao obter produtos de idCategoriaProduto: {idCategoriaProduto}. Erro: {ex.Message}.");
                throw;
            }
        }
    }

}
