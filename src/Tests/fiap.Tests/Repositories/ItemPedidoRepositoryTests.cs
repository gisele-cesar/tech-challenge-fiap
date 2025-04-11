using fiap.Domain.Entities;
using fiap.Domain.Interfaces;
using fiap.Repositories;
using Moq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace fiap.Tests.Repositories
{
    public class ItemPedidoRepositoryTests
    {
        private readonly Pedido pedido;
        private readonly List<Pedido> lstPedido = [];
        public ItemPedidoRepositoryTests()
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
        public void ObterItemPedidoTest()
        {
            var _clienteRepository = new Mock<IClienteRepository>();
            var _repo = new Mock<Func<IDbConnection>>();
            var _logger = new Mock<Serilog.ILogger>();
            var readerMock = new Mock<IDataReader>();

            _clienteRepository.SetupSequence(_=>_.Obter(It.IsAny<int>())).Returns( Task.FromResult( new Cliente { Cpf = "1234579" , Email = "ts@t.com" , Id = 1, Nome = "teste" }));

            readerMock.SetupSequence(_ => _.Read())
                .Returns(true)
                .Returns(false);

            readerMock.SetupSequence(reader => reader["IdProduto"]).Returns(1);
            readerMock.SetupSequence(reader => reader["IdCategoriaProduto"]).Returns(1);
            readerMock.SetupSequence(reader => reader["Nome"]).Returns("teste");
            readerMock.SetupSequence(reader => reader["Descricao"]).Returns("teste");
            readerMock.SetupSequence(reader => reader["Preco"]).Returns((decimal)10.50); 


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

            var data = new ItemPedidoRepository(_logger.Object, _repo.Object, _clienteRepository.Object);

            //Act
            var result = data.ObterItemPedido(1);

            Assert.NotNull(result);
        }
    }
}