using fiap.Domain.Entities;
using fiap.Repositories;
using Moq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Xunit;

namespace fiap.Tests.Repositories
{
    public class ProdutoRepositoryTests
    {
        private readonly Produto produto;
        private readonly List<Produto> lstProduto = [];
        public ProdutoRepositoryTests()
        {
            produto = new Produto
            {
                Preco = (decimal)2.1,
                Nome = "teste2",
                IdProduto = 1,
                IdCategoriaProduto = 1,
                Descricao = "teste2",
                DataCriacao = DateTime.Now,
                DataAlteracao = DateTime.Now
            };

            lstProduto = [
                      new() { Preco = 1 , Nome = "teste" , IdProduto = 1, IdCategoriaProduto = 1 , Descricao = "teste" , DataCriacao = DateTime.Now , DataAlteracao = DateTime.Now },
                      new() { Preco = 2 , Nome = "teste2" , IdProduto = 2, IdCategoriaProduto = 2 , Descricao = "teste2" , DataCriacao = DateTime.Now , DataAlteracao = DateTime.Now }
                  ];
           
        }
        [Fact]
        public async void ObterProdutoPorId_TestAsync()
        {
            var _repo = new Mock<Func<IDbConnection>>();
            var _logger = new Mock<Serilog.ILogger>();
            var readerMock = new Mock<IDataReader>();
            var parameterMock = new Mock<IDbDataParameter>();
            
            parameterMock.SetupSequence(x=>x.ParameterName).Returns("@id");
            parameterMock.SetupSequence(x => x.Value).Returns(1);

            List<DbParameter> lstParameter = new List<DbParameter>();

            SqlCommand cmd = new SqlCommand();
            lstParameter.Add(cmd.CreateParameter());

            readerMock.SetupSequence(_ => _.Read())
              .Returns(true)
              .Returns(false);

            readerMock.Setup(reader => reader["IdProduto"]).Returns(1);
            readerMock.Setup(reader => reader["IdCategoriaProduto"]).Returns(1);
            readerMock.Setup(reader => reader["Nome"]).Returns("teste");
            readerMock.Setup(reader => reader["Descricao"]).Returns("teste");
            readerMock.Setup(reader => reader["Preco"]).Returns((decimal)11.10);
            readerMock.Setup(reader => reader["DataCriacao"]).Returns(DateTime.Now);
            readerMock.Setup(reader => reader["DataAlteracao"]).Returns(DateTime.Now);

            var commandMock = new Mock<IDbCommand>();

            commandMock.Setup(m => m.CreateParameter()).Returns(lstParameter[0]);
            commandMock.Setup(m => m.Parameters.Add(cmd.CreateParameter()));

            var connectionMock = new Mock<IDbConnection>();
            commandMock.Setup(m => m.ExecuteReader()).Returns(readerMock.Object).Verifiable();
            connectionMock.SetupSequence(m => m.CreateCommand()).Returns(commandMock.Object);

            _repo.Setup(a => a.Invoke()).Returns(connectionMock.Object);

            var data = new ProdutoRepository(_logger.Object, _repo.Object);

            //Act
            var result = await data.Obter(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async void ObterProduto_TestAsync()
        {
            var _repo = new Mock<Func<IDbConnection>>();
            var _logger = new Mock<Serilog.ILogger>();
            var readerMock = new Mock<IDataReader>();
            var parameterMock = new Mock<IDbDataParameter>();

            parameterMock.SetupSequence(x => x.ParameterName).Returns("@id");
            parameterMock.SetupSequence(x => x.Value).Returns(1);

            List<DbParameter> lstParameter = new List<DbParameter>();

            SqlCommand cmd = new SqlCommand();
            lstParameter.Add(cmd.CreateParameter());

            readerMock.SetupSequence(_ => _.Read())
              .Returns(true)
              .Returns(false);

            readerMock.Setup(reader => reader["IdProduto"]).Returns(1);
            readerMock.Setup(reader => reader["IdCategoriaProduto"]).Returns(1);
            readerMock.Setup(reader => reader["Nome"]).Returns("teste");
            readerMock.Setup(reader => reader["Descricao"]).Returns("teste");
            readerMock.Setup(reader => reader["Preco"]).Returns((decimal)11.10);
            readerMock.Setup(reader => reader["DataCriacao"]).Returns(DateTime.Now);
            readerMock.Setup(reader => reader["DataAlteracao"]).Returns(DateTime.Now);

            var commandMock = new Mock<IDbCommand>();

            commandMock.Setup(m => m.CreateParameter()).Returns(lstParameter[0]);
            commandMock.Setup(m => m.Parameters.Add(cmd.CreateParameter()));

            var connectionMock = new Mock<IDbConnection>();
            commandMock.Setup(m => m.ExecuteReader()).Returns(readerMock.Object).Verifiable();
            connectionMock.SetupSequence(m => m.CreateCommand()).Returns(commandMock.Object);

            _repo.Setup(a => a.Invoke()).Returns(connectionMock.Object);

            var data = new ProdutoRepository(_logger.Object, _repo.Object);

            //Act
            var result = await data.Obter();

            Assert.NotNull(result);
        }

        [Fact]
        public async void ObterProdutoPorCategoria_TestAsync()
        {
            var _repo = new Mock<Func<IDbConnection>>();
            var _logger = new Mock<Serilog.ILogger>();
            var readerMock = new Mock<IDataReader>();
            var parameterMock = new Mock<IDbDataParameter>();

            parameterMock.SetupSequence(x => x.ParameterName).Returns("@id");
            parameterMock.SetupSequence(x => x.Value).Returns(1);

            List<DbParameter> lstParameter = new List<DbParameter>();

            SqlCommand cmd = new SqlCommand();
            lstParameter.Add(cmd.CreateParameter());

            readerMock.SetupSequence(_ => _.Read())
              .Returns(true)
              .Returns(false);

            readerMock.Setup(reader => reader["IdProduto"]).Returns(1);
            readerMock.Setup(reader => reader["IdCategoriaProduto"]).Returns(1);
            readerMock.Setup(reader => reader["Nome"]).Returns("teste");
            readerMock.Setup(reader => reader["Descricao"]).Returns("teste");
            readerMock.Setup(reader => reader["Preco"]).Returns((decimal)11.10);
            readerMock.Setup(reader => reader["DataCriacao"]).Returns(DateTime.Now);
            readerMock.Setup(reader => reader["DataAlteracao"]).Returns(DateTime.Now);

            var commandMock = new Mock<IDbCommand>();

            commandMock.Setup(m => m.CreateParameter()).Returns(lstParameter[0]);
            commandMock.Setup(m => m.Parameters.Add(cmd.CreateParameter()));

            var connectionMock = new Mock<IDbConnection>();
            commandMock.Setup(m => m.ExecuteReader()).Returns(readerMock.Object).Verifiable();
            connectionMock.SetupSequence(m => m.CreateCommand()).Returns(commandMock.Object);

            _repo.Setup(a => a.Invoke()).Returns(connectionMock.Object);

            var data = new ProdutoRepository(_logger.Object, _repo.Object);

            //Act
            var result = await data.ObterProdutosPorCategoria(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async void Inserir_TestAsync()
        {
            var _repo = new Mock<Func<IDbConnection>>();
            var _logger = new Mock<Serilog.ILogger>();
            var readerMock = new Mock<IDataReader>();
            var parameterMock = new Mock<IDbDataParameter>();

            parameterMock.SetupSequence(x => x.ParameterName).Returns("@id");
            parameterMock.SetupSequence(x => x.Value).Returns(1);

            List<DbParameter> lstParameter = new List<DbParameter>();

            SqlCommand cmd = new SqlCommand();
            lstParameter.Add(cmd.CreateParameter());

            readerMock.SetupSequence(_ => _.Read())
              .Returns(true)
              .Returns(false);

            readerMock.Setup(reader => reader["IdProduto"]).Returns(1);
            readerMock.Setup(reader => reader["IdCategoriaProduto"]).Returns(1);
            readerMock.Setup(reader => reader["Nome"]).Returns("teste");
            readerMock.Setup(reader => reader["Descricao"]).Returns("teste");
            readerMock.Setup(reader => reader["Preco"]).Returns((decimal)11.10);
            readerMock.Setup(reader => reader["DataCriacao"]).Returns(DateTime.Now);
            readerMock.Setup(reader => reader["DataAlteracao"]).Returns(DateTime.Now);

            var commandMock = new Mock<IDbCommand>();

            commandMock.Setup(m => m.CreateParameter()).Returns(lstParameter[0]);
            commandMock.Setup(m => m.Parameters.Add(cmd.CreateParameter()));

            var connectionMock = new Mock<IDbConnection>();
            commandMock.Setup(m => m.ExecuteNonQuery()).Returns(1).Verifiable();
            connectionMock.SetupSequence(m => m.CreateCommand()).Returns(commandMock.Object);

            _repo.Setup(a => a.Invoke()).Returns(connectionMock.Object);

            var data = new ProdutoRepository(_logger.Object, _repo.Object);

            //Act
            var result = await data.Inserir(produto);

            Assert.True(result);
        }

        [Fact]
        public async void Atualizar_TestAsync()
        {
            var _repo = new Mock<Func<IDbConnection>>();
            var _logger = new Mock<Serilog.ILogger>();
            var readerMock = new Mock<IDataReader>();
            var parameterMock = new Mock<IDbDataParameter>();

            parameterMock.SetupSequence(x => x.ParameterName).Returns("@id");
            parameterMock.SetupSequence(x => x.Value).Returns(1);

            List<DbParameter> lstParameter = new List<DbParameter>();

            SqlCommand cmd = new SqlCommand();
            lstParameter.Add(cmd.CreateParameter());

            readerMock.SetupSequence(_ => _.Read())
              .Returns(true)
              .Returns(false);

            readerMock.Setup(reader => reader["IdProduto"]).Returns(1);
            readerMock.Setup(reader => reader["IdCategoriaProduto"]).Returns(1);
            readerMock.Setup(reader => reader["Nome"]).Returns("teste");
            readerMock.Setup(reader => reader["Descricao"]).Returns("teste");
            readerMock.Setup(reader => reader["Preco"]).Returns((decimal)11.10);
            readerMock.Setup(reader => reader["DataCriacao"]).Returns(DateTime.Now);
            readerMock.Setup(reader => reader["DataAlteracao"]).Returns(DateTime.Now);

            var commandMock = new Mock<IDbCommand>();

            commandMock.Setup(m => m.CreateParameter()).Returns(lstParameter[0]);
            commandMock.Setup(m => m.Parameters.Add(cmd.CreateParameter()));

            var connectionMock = new Mock<IDbConnection>();
            commandMock.Setup(m => m.ExecuteNonQuery()).Returns(1).Verifiable();
            connectionMock.SetupSequence(m => m.CreateCommand()).Returns(commandMock.Object);

            _repo.Setup(a => a.Invoke()).Returns(connectionMock.Object);

            var data = new ProdutoRepository(_logger.Object, _repo.Object);

            //Act
            var result = await data.Atualizar(produto);

            Assert.True(result);
        }

    }
}