using fiap.Domain.Entities;
using fiap.Domain.Repositories.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace fiap.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly Func<IDbConnection> _connectionFactory;

        public ClienteRepository(Func<IDbConnection> connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public Task<Cliente> ObterPorId(int id)
        {
            using var connection = _connectionFactory();
            connection.Open();
            // Execute your query
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Cliente WHERE IdCliente = @id";
            var param = command.CreateParameter();
            param.ParameterName = "@id";
            param.Value = id;
            command.Parameters.Add(param);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                // Map your data to your entity
                return Task.FromResult(new Cliente
                {
                    Id = (int)reader["IdCliente"],
                    // Map other fields
                });
            }
            else
            {
                return (Task<Cliente>)Task.CompletedTask;
            }
        }

        public Task<bool> Salvar(Cliente cliente)
        {
            using var connection = _connectionFactory();
            connection.Open();
            // Execute your query
            using var command = connection.CreateCommand();
            command.CommandText = "insert into Cliente values (@nome, @cpf, @email, @data)";

            command.Parameters.Add(new SqlParameter { ParameterName = "@nome", Value = cliente.Nome , SqlDbType = SqlDbType.VarChar });
            command.Parameters.Add(new SqlParameter { ParameterName = "@cpf", Value = cliente.Cpf, SqlDbType = SqlDbType.VarChar });
            command.Parameters.Add(new SqlParameter { ParameterName = "@email", Value = cliente.Email, SqlDbType = SqlDbType.VarChar });
            command.Parameters.Add(new SqlParameter { ParameterName = "@data", Value = DateTime.Now, SqlDbType = SqlDbType.DateTime });

            return Task.FromResult(command.ExecuteNonQuery() >= 1);
          
        }
    }

}
