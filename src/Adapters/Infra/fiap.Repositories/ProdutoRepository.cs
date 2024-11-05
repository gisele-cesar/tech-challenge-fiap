using fiap.Domain.Entities;
using fiap.Domain.Repositories.Interfaces;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace fiap.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly Func<IDbConnection> _connectionFactory;

        public ProdutoRepository(Func<IDbConnection> connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public Task<Produto> Obter(int id)
        {
            using var connection = _connectionFactory();
            connection.Open();
            // Execute your query
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Produto WHERE IdProduto = @id";
            var param = command.CreateParameter();
            param.ParameterName = "@id";
            param.Value = id;
            command.Parameters.Add(param);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                // Map your data to your entity
                return Task.FromResult(new Produto
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
            else
            {
                return (Task<Produto>)Task.CompletedTask;
            }
        }

        public Task<bool> Inserir(Produto Produto)
        {
            using var connection = _connectionFactory();
            connection.Open();
            // Execute your query
            using var command = connection.CreateCommand();
            command.CommandText = "insert into Produto values (@idCategoriaProduto, @nome, @descricao, @preco, @dataCriacao, @dataAlteracao)";

            command.Parameters.Add(new SqlParameter { ParameterName = "@idCategoriaProduto", Value = Produto.IdCategoriaProduto, SqlDbType = SqlDbType.Int });
            command.Parameters.Add(new SqlParameter { ParameterName = "@nome", Value = Produto.Nome, SqlDbType = SqlDbType.VarChar });
            command.Parameters.Add(new SqlParameter { ParameterName = "@descricao", Value = Produto.Descricao, SqlDbType = SqlDbType.VarChar });
            command.Parameters.Add(new SqlParameter { ParameterName = "@preco", Value = Produto.Preco, SqlDbType = SqlDbType.Decimal });
            command.Parameters.Add(new SqlParameter { ParameterName = "@dataCriacao", Value = DateTime.Now, SqlDbType = SqlDbType.DateTime });
            command.Parameters.Add(new SqlParameter { ParameterName = "@dataAlteracao", Value = System.DBNull.Value, SqlDbType = SqlDbType.DateTime });

            return Task.FromResult(command.ExecuteNonQuery() >= 1);

        }
        public Task<bool> Atualizar(Produto Produto)
        {
            using var connection = _connectionFactory();
            StringBuilder sb = new StringBuilder();
            connection.Open();
            // Execute your query
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

            return Task.FromResult(command.ExecuteNonQuery() >= 1);

        }
        public Task<List<Produto>> Obter()
        {
            using var connection = _connectionFactory();
            connection.Open();
            var lst = new List<Produto>();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Produto";

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
                    Preco = (decimal)reader["Preco"],
                    DataCriacao = (DateTime)reader["DataCriacao"],
                    DataAlteracao = reader["DataAlteracao"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["DataAlteracao"]
                });
            }
            return Task.FromResult(lst);
        }

        public Task<List<Produto>> ObterProdutosPorCategoria(int idCategoriaProduto)
        {
            using var connection = _connectionFactory();
            connection.Open();
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
            return Task.FromResult(lst);
        }
    }

}
