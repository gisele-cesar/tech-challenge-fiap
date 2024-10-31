using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace fiap.Repositories
{
    public class ConexaoBanco
    {
        private readonly IConfiguration _config;
        public ConexaoBanco(IConfiguration config)
        {
            _config = config;    
        }
        public IDbConnection ObterConexao()
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("fiap.sqlServer"));
            dbConnection.Open();
            return dbConnection;
        }
    }
}
