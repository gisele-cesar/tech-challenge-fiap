using fiap.Domain.Entities;
using fiap.Domain.Interfaces;
using fiap.Repositories;
using Moq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Xunit;

namespace fiap.Tests.Repositories
{
    public class PedidoRepositoryTests
    {
        private readonly Pedido pedido;
        private readonly List<Pedido> lstPedido = [];
        public PedidoRepositoryTests()
        {
            pedido = new Pedido
            {
                Cliente = new Cliente { Cpf = "12345678910", Email = "teste@teste.com", Id = 1, Nome = "Joao da Silva" },
                IdPedido = 1,
                Numero = "123456",
                StatusPagamento = new StatusPagamento { Descricao = "xx", IdStatusPagamento = 1 }
                  ,
                StatusPedido = new StatusPedido { IdStatusPedido = 1, Descricao = "xx" }
                  ,
                Produtos = [
                      new() { Preco = 1 , Nome = "teste" , IdProduto = 1, IdCategoriaProduto = 1 , Descricao = "teste" , DataCriacao = DateTime.Now , DataAlteracao = DateTime.Now },
                      new() { Preco = 2 , Nome = "teste2" , IdProduto = 2, IdCategoriaProduto = 2 , Descricao = "teste2" , DataCriacao = DateTime.Now , DataAlteracao = DateTime.Now }
                  ]
            };
            lstPedido = [
                pedido,
                pedido
            ];
        }
        [Fact]
        public void ObterPedidoTest()
        {
            var _clienteRepository = new Mock<IClienteRepository>();
            var _itemPedidoRepository = new Mock<IItemPedidoRepository>();
            var _repo = new Mock<Func<IDbConnection>>();
            var _logger = new Mock<Serilog.ILogger>();
            var readerMock = new Mock<IDataReader>();

            _clienteRepository.SetupSequence(_=>_.Obter(It.IsAny<int>())).Returns( Task.FromResult( new Cliente { Cpf = "1234579" , Email = "ts@t.com" , Id = 1, Nome = "teste" }));


            var itemPedido = new List<Produto> { new() { Preco = 1, Nome = "teste", IdProduto = 1, IdCategoriaProduto = 1, Descricao = "teste", DataCriacao = DateTime.Now, DataAlteracao = DateTime.Now } };
            _itemPedidoRepository.Setup(x => x.ObterItemPedido(It.IsAny<int>())).Returns(Task.FromResult(itemPedido));

            readerMock.SetupSequence(_ => _.Read())
                .Returns(true)
                .Returns(false);

            readerMock.SetupSequence(reader => reader["IdPedido"]).Returns(1);
            readerMock.SetupSequence(reader => reader["IdCliente"]).Returns(1);
            readerMock.SetupSequence(reader => reader["NumeroPedido"]).Returns(1);
            readerMock.SetupSequence(reader => reader["IdStatusPedido"]).Returns(1);
            readerMock.SetupSequence(reader => reader["DescricaoStatusPedido"]).Returns("teste");
            readerMock.SetupSequence(reader => reader["IdStatusPagamento"]).Returns(1);
            readerMock.SetupSequence(reader => reader["DescricaoStatusPagamento"]).Returns("teste");
            readerMock.SetupSequence(reader => reader["IdPedido"]).Returns(1); 


            var commandMock = new Mock<IDbCommand>();

            commandMock.Setup(m => m.ExecuteReader()).Returns(readerMock.Object).Verifiable();

            var parameterMock = new Mock<IDbDataParameter>();
            
            parameterMock.SetupSequence(x=>x.ParameterName).Returns("@id");
            parameterMock.SetupSequence(x => x.Value).Returns("1");

            List<DbParameter> lstParameter = new List<DbParameter>();

            SqlCommand cmd = new SqlCommand();
            lstParameter.Add(cmd.CreateParameter());

            commandMock.Setup(m => m.CreateParameter()).Returns(lstParameter[0]);
            commandMock.Setup(m => m.Parameters.Add(cmd.CreateParameter()));

            var connectionMock = new Mock<IDbConnection>();
            connectionMock.SetupSequence(m => m.CreateCommand()).Returns(commandMock.Object);

            _repo.Setup(a => a.Invoke()).Returns(connectionMock.Object);

            var data = new PedidoRepository(_logger.Object, _repo.Object, _clienteRepository.Object, _itemPedidoRepository.Object);

            //Act
            var result = data.ObterPedido(1);

            Assert.NotNull(result);
        }

        [Fact]
        public void ObterPedidoPorStatusTest()
        {
            var _clienteRepository = new Mock<IClienteRepository>();
            var _itemPedidoRepository = new Mock<IItemPedidoRepository>();
            var _repo = new Mock<Func<IDbConnection>>();
            var _logger = new Mock<Serilog.ILogger>();
            var readerMock = new Mock<IDataReader>();

            _clienteRepository.SetupSequence(_ => _.Obter(It.IsAny<int>())).Returns(Task.FromResult(new Cliente { Cpf = "1234579", Email = "ts@t.com", Id = 1, Nome = "teste" }));


            var itemPedido = new List<Produto> { new() { Preco = 1, Nome = "teste", IdProduto = 1, IdCategoriaProduto = 1, Descricao = "teste", DataCriacao = DateTime.Now, DataAlteracao = DateTime.Now } };
           
            _itemPedidoRepository.Setup(x => x.ObterItemPedido(It.IsAny<int>())).Returns(Task.FromResult(itemPedido));

            readerMock.SetupSequence(_ => _.Read())
                .Returns(true);
                ///  .Returns(false);

            readerMock.SetupSequence(reader => reader["IdPedido"]).Returns(1);
            readerMock.SetupSequence(reader => reader["IdCliente"]).Returns(1);
            readerMock.SetupSequence(reader => reader["NumeroPedido"]).Returns(1);
            readerMock.SetupSequence(reader => reader["IdStatusPedido"]).Returns(1);
            readerMock.SetupSequence(reader => reader["DescricaoStatusPedido"]).Returns("teste");
            readerMock.SetupSequence(reader => reader["IdStatusPagamento"]).Returns(1);
            readerMock.SetupSequence(reader => reader["DescricaoStatusPagamento"]).Returns("teste");


            var commandMock = new Mock<IDbCommand>();

            commandMock.Setup(m => m.ExecuteReader()).Returns(readerMock.Object).Verifiable();

            var parameterMock = new Mock<IDbDataParameter>();

            parameterMock.SetupSequence(x => x.ParameterName).Returns("@id");
            parameterMock.SetupSequence(x => x.Value).Returns("1");

            List<DbParameter> lstParameter = new List<DbParameter>();

            SqlCommand cmd = new SqlCommand();
            lstParameter.Add(cmd.CreateParameter());

            commandMock.Setup(m => m.CreateParameter()).Returns(lstParameter[0]);
            commandMock.Setup(m => m.Parameters.Add(cmd.CreateParameter()));

            var connectionMock = new Mock<IDbConnection>();
            connectionMock.SetupSequence(m => m.CreateCommand()).Returns(commandMock.Object);

            _repo.Setup(a => a.Invoke()).Returns(connectionMock.Object);

            var data = new PedidoRepository(_logger.Object, _repo.Object, _clienteRepository.Object, _itemPedidoRepository.Object);

            //Act
            var result = data.ObterPedidosPorStatus("1","2","3");

            Assert.NotNull(result);
        }

        [Fact]
        public void ObterPedidoPorIdPedidoTests()
        {
            var _clienteRepository = new Mock<IClienteRepository>();
            var _itemPedidoRepository = new Mock<IItemPedidoRepository>();
            var _repo = new Mock<Func<IDbConnection>>();
            var _logger = new Mock<Serilog.ILogger>();
            var readerMock = new Mock<IDataReader>();

            _clienteRepository.SetupSequence(_ => _.Obter(It.IsAny<int>())).Returns(Task.FromResult(new Cliente { Cpf = "1234579", Email = "ts@t.com", Id = 1, Nome = "teste" }));


            var itemPedido = new List<Produto> { new() { Preco = 1, Nome = "teste", IdProduto = 1, IdCategoriaProduto = 1, Descricao = "teste", DataCriacao = DateTime.Now, DataAlteracao = DateTime.Now } };

            _itemPedidoRepository.Setup(x => x.ObterItemPedido(It.IsAny<int>())).Returns(Task.FromResult(itemPedido));

            readerMock.SetupSequence(_ => _.Read())
                .Returns(true);
            ///  .Returns(false);

            readerMock.SetupSequence(reader => reader["IdPedido"]).Returns(1);
            readerMock.SetupSequence(reader => reader["IdCliente"]).Returns(1);
            readerMock.SetupSequence(reader => reader["NumeroPedido"]).Returns(1);
            readerMock.SetupSequence(reader => reader["IdStatusPedido"]).Returns(1);
            readerMock.SetupSequence(reader => reader["DescricaoStatusPedido"]).Returns("teste");
            readerMock.SetupSequence(reader => reader["IdStatusPagamento"]).Returns(1);
            readerMock.SetupSequence(reader => reader["DescricaoStatusPagamento"]).Returns("teste");


            var commandMock = new Mock<IDbCommand>();

            commandMock.Setup(m => m.ExecuteReader()).Returns(readerMock.Object).Verifiable();

            var parameterMock = new Mock<IDbDataParameter>();

            parameterMock.SetupSequence(x => x.ParameterName).Returns("@id");
            parameterMock.SetupSequence(x => x.Value).Returns("1");

            List<DbParameter> lstParameter = new List<DbParameter>();

            SqlCommand cmd = new SqlCommand();
            lstParameter.Add(cmd.CreateParameter());

            commandMock.Setup(m => m.CreateParameter()).Returns(lstParameter[0]);
            commandMock.Setup(m => m.Parameters.Add(cmd.CreateParameter()));

            var connectionMock = new Mock<IDbConnection>();
            connectionMock.SetupSequence(m => m.CreateCommand()).Returns(commandMock.Object);

            _repo.Setup(a => a.Invoke()).Returns(connectionMock.Object);

            var data = new PedidoRepository(_logger.Object, _repo.Object, _clienteRepository.Object, _itemPedidoRepository.Object);

            //Act
            var result = data.ObterPedido(1);

            Assert.NotNull(result);
        }

        [Fact]
        public void Inserir_Tests()
        {
            var _clienteRepository = new Mock<IClienteRepository>();
            var _itemPedidoRepository = new Mock<IItemPedidoRepository>();
            var _repo = new Mock<Func<IDbConnection>>();
            var _logger = new Mock<Serilog.ILogger>();
            var readerMock = new Mock<IDataReader>();

            _clienteRepository.SetupSequence(_ => _.Obter(It.IsAny<int>())).Returns(Task.FromResult(new Cliente { Cpf = "1234579", Email = "ts@t.com", Id = 1, Nome = "teste" }));


            var itemPedido = new List<Produto> { new() { Preco = 1, Nome = "teste", IdProduto = 1, IdCategoriaProduto = 1, Descricao = "teste", DataCriacao = DateTime.Now, DataAlteracao = DateTime.Now } };

            _itemPedidoRepository.Setup(x => x.ObterItemPedido(It.IsAny<int>())).Returns(Task.FromResult(itemPedido));

            readerMock.SetupSequence(_ => _.Read())
                .Returns(true);
            ///  .Returns(false);

            readerMock.SetupSequence(reader => reader["IdPedido"]).Returns(1);
            readerMock.SetupSequence(reader => reader["IdCliente"]).Returns(1);
            readerMock.SetupSequence(reader => reader["NumeroPedido"]).Returns(1);
            readerMock.SetupSequence(reader => reader["IdStatusPedido"]).Returns(1);
            readerMock.SetupSequence(reader => reader["DescricaoStatusPedido"]).Returns("teste");
            readerMock.SetupSequence(reader => reader["IdStatusPagamento"]).Returns(1);
            readerMock.SetupSequence(reader => reader["DescricaoStatusPagamento"]).Returns("teste");


            var commandMock = new Mock<IDbCommand>();
            var commandMock2 = new Mock<IDbCommand>();

            commandMock.Setup(m => m.ExecuteScalar()).Returns(1).Verifiable();
            commandMock2.Setup(m => m.ExecuteScalar()).Returns(1).Verifiable();

            commandMock.Setup(m => m.ExecuteNonQuery()).Returns(1).Verifiable();
            commandMock2.Setup(m => m.ExecuteNonQuery()).Returns(1).Verifiable();

            var parameterMock = new Mock<IDbDataParameter>();

            parameterMock.SetupSequence(x => x.ParameterName).Returns("@id");
            parameterMock.SetupSequence(x => x.Value).Returns("1");

            List<DbParameter> lstParameter = new List<DbParameter>();

            SqlCommand cmd = new SqlCommand();
            lstParameter.Add(cmd.CreateParameter());

            commandMock.Setup(m => m.CreateParameter()).Returns(lstParameter[0]);
            commandMock.Setup(m => m.Parameters.Add(cmd.CreateParameter()));

            commandMock2.Setup(m => m.CreateParameter()).Returns(lstParameter[0]);
            commandMock2.Setup(m => m.Parameters.Add(cmd.CreateParameter()));

            var connectionMock = new Mock<IDbConnection>();
            var connectionMock2 = new Mock<IDbConnection>();
            var transactionMock = new Mock<IDbTransaction>();

            //// transactionMock.Setup(x=>x.co)

            connectionMock.SetupSequence(m => m.CreateCommand()).Returns(commandMock.Object);
            connectionMock2.SetupSequence(m => m.CreateCommand()).Returns(commandMock2.Object);
            connectionMock.SetupSequence(m => m.BeginTransaction()).Returns(Mock.Of<IDbTransaction>());
            connectionMock2.SetupSequence(m => m.BeginTransaction()).Returns(Mock.Of<IDbTransaction>());

            _repo.SetupSequence(a => a.Invoke()).Returns(connectionMock.Object);
            _repo.SetupSequence(a => a.Invoke()).Returns(connectionMock2.Object);

            var data = new PedidoRepository(_logger.Object, _repo.Object, _clienteRepository.Object, _itemPedidoRepository.Object);

            //Act
            var result = data.Inserir(pedido);

            Assert.NotNull(result);
        }

        [Fact]
        public void AtualizarStatusPedido_Tests()
        {
            var _clienteRepository = new Mock<IClienteRepository>();
            var _itemPedidoRepository = new Mock<IItemPedidoRepository>();
            var _repo = new Mock<Func<IDbConnection>>();
            var _logger = new Mock<Serilog.ILogger>();
            var readerMock = new Mock<IDataReader>();

            _clienteRepository.SetupSequence(_ => _.Obter(It.IsAny<int>())).Returns(Task.FromResult(new Cliente { Cpf = "1234579", Email = "ts@t.com", Id = 1, Nome = "teste" }));


            var itemPedido = new List<Produto> { new() { Preco = 1, Nome = "teste", IdProduto = 1, IdCategoriaProduto = 1, Descricao = "teste", DataCriacao = DateTime.Now, DataAlteracao = DateTime.Now } };

            _itemPedidoRepository.Setup(x => x.ObterItemPedido(It.IsAny<int>())).Returns(Task.FromResult(itemPedido));

            readerMock.SetupSequence(_ => _.Read())
                .Returns(true);
            ///  .Returns(false);

            readerMock.SetupSequence(reader => reader["IdPedido"]).Returns(1);
            readerMock.SetupSequence(reader => reader["IdCliente"]).Returns(1);
            readerMock.SetupSequence(reader => reader["NumeroPedido"]).Returns(1);
            readerMock.SetupSequence(reader => reader["IdStatusPedido"]).Returns(1);
            readerMock.SetupSequence(reader => reader["DescricaoStatusPedido"]).Returns("teste");
            readerMock.SetupSequence(reader => reader["IdStatusPagamento"]).Returns(1);
            readerMock.SetupSequence(reader => reader["DescricaoStatusPagamento"]).Returns("teste");


            var commandMock = new Mock<IDbCommand>();
            var commandMock2 = new Mock<IDbCommand>();

            commandMock.Setup(m => m.ExecuteScalar()).Returns(1).Verifiable();
            commandMock2.Setup(m => m.ExecuteScalar()).Returns(1).Verifiable();

            commandMock.Setup(m => m.ExecuteNonQuery()).Returns(1).Verifiable();
            commandMock2.Setup(m => m.ExecuteNonQuery()).Returns(1).Verifiable();

            var parameterMock = new Mock<IDbDataParameter>();

            parameterMock.SetupSequence(x => x.ParameterName).Returns("@id");
            parameterMock.SetupSequence(x => x.Value).Returns("1");

            List<DbParameter> lstParameter = new List<DbParameter>();

            SqlCommand cmd = new SqlCommand();
            lstParameter.Add(cmd.CreateParameter());

            commandMock.Setup(m => m.CreateParameter()).Returns(lstParameter[0]);
            commandMock.Setup(m => m.Parameters.Add(cmd.CreateParameter()));

            commandMock2.Setup(m => m.CreateParameter()).Returns(lstParameter[0]);
            commandMock2.Setup(m => m.Parameters.Add(cmd.CreateParameter()));

            var connectionMock = new Mock<IDbConnection>();
            var connectionMock2 = new Mock<IDbConnection>();
            var transactionMock = new Mock<IDbTransaction>();

            //// transactionMock.Setup(x=>x.co)

            connectionMock.SetupSequence(m => m.CreateCommand()).Returns(commandMock.Object);
            connectionMock2.SetupSequence(m => m.CreateCommand()).Returns(commandMock2.Object);
            connectionMock.SetupSequence(m => m.BeginTransaction()).Returns(Mock.Of<IDbTransaction>());
            connectionMock2.SetupSequence(m => m.BeginTransaction()).Returns(Mock.Of<IDbTransaction>());

            _repo.SetupSequence(a => a.Invoke()).Returns(connectionMock.Object);
            _repo.SetupSequence(a => a.Invoke()).Returns(connectionMock2.Object);

            var data = new PedidoRepository(_logger.Object, _repo.Object, _clienteRepository.Object, _itemPedidoRepository.Object);

            //Act
            var result = data.AtualizarStatusPedido(pedido);

            Assert.NotNull(result);
        }

        [Fact]
        public void Atualizar_Tests()
        {
            var _clienteRepository = new Mock<IClienteRepository>();
            var _itemPedidoRepository = new Mock<IItemPedidoRepository>();
            var _repo = new Mock<Func<IDbConnection>>();
            var _logger = new Mock<Serilog.ILogger>();
            var readerMock = new Mock<IDataReader>();

            _clienteRepository.SetupSequence(_ => _.Obter(It.IsAny<int>())).Returns(Task.FromResult(new Cliente { Cpf = "1234579", Email = "ts@t.com", Id = 1, Nome = "teste" }));


            var itemPedido = new List<Produto> { new() { Preco = 1, Nome = "teste", IdProduto = 1, IdCategoriaProduto = 1, Descricao = "teste", DataCriacao = DateTime.Now, DataAlteracao = DateTime.Now } };

            _itemPedidoRepository.Setup(x => x.ObterItemPedido(It.IsAny<int>())).Returns(Task.FromResult(itemPedido));

            readerMock.SetupSequence(_ => _.Read())
                .Returns(true);
            ///  .Returns(false);

            readerMock.SetupSequence(reader => reader["IdPedido"]).Returns(1);
            readerMock.SetupSequence(reader => reader["IdCliente"]).Returns(1);
            readerMock.SetupSequence(reader => reader["NumeroPedido"]).Returns(1);
            readerMock.SetupSequence(reader => reader["IdStatusPedido"]).Returns(1);
            readerMock.SetupSequence(reader => reader["DescricaoStatusPedido"]).Returns("teste");
            readerMock.SetupSequence(reader => reader["IdStatusPagamento"]).Returns(1);
            readerMock.SetupSequence(reader => reader["DescricaoStatusPagamento"]).Returns("teste");


            var commandMock = new Mock<IDbCommand>();
            var commandMock2 = new Mock<IDbCommand>();

            commandMock.Setup(m => m.ExecuteScalar()).Returns(1).Verifiable();
            commandMock2.Setup(m => m.ExecuteScalar()).Returns(1).Verifiable();

            commandMock.Setup(m => m.ExecuteNonQuery()).Returns(1).Verifiable();
            commandMock2.Setup(m => m.ExecuteNonQuery()).Returns(1).Verifiable();

            var parameterMock = new Mock<IDbDataParameter>();

            parameterMock.SetupSequence(x => x.ParameterName).Returns("@id");
            parameterMock.SetupSequence(x => x.Value).Returns("1");

            List<DbParameter> lstParameter = new List<DbParameter>();

            SqlCommand cmd = new SqlCommand();
            lstParameter.Add(cmd.CreateParameter());

            commandMock.Setup(m => m.CreateParameter()).Returns(lstParameter[0]);
            commandMock.Setup(m => m.Parameters.Add(cmd.CreateParameter()));

            commandMock2.Setup(m => m.CreateParameter()).Returns(lstParameter[0]);
            commandMock2.Setup(m => m.Parameters.Add(cmd.CreateParameter()));

            var connectionMock = new Mock<IDbConnection>();
            var connectionMock2 = new Mock<IDbConnection>();
            var transactionMock = new Mock<IDbTransaction>();

            //// transactionMock.Setup(x=>x.co)

            connectionMock.SetupSequence(m => m.CreateCommand()).Returns(commandMock.Object);
            connectionMock2.SetupSequence(m => m.CreateCommand()).Returns(commandMock2.Object);
            connectionMock.SetupSequence(m => m.BeginTransaction()).Returns(Mock.Of<IDbTransaction>());
            connectionMock2.SetupSequence(m => m.BeginTransaction()).Returns(Mock.Of<IDbTransaction>());

            _repo.SetupSequence(a => a.Invoke()).Returns(connectionMock.Object);
            _repo.SetupSequence(a => a.Invoke()).Returns(connectionMock2.Object);

            var data = new PedidoRepository(_logger.Object, _repo.Object, _clienteRepository.Object, _itemPedidoRepository.Object);

            //Act
            var result = data.Atualizar(pedido);

            Assert.NotNull(result);
        }
    }
}