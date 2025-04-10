using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fiap.Domain.Entities;
using fiap.Domain.Interfaces;
using Moq;
using System.Data;
using fiap.Application.UseCases;
using static System.Runtime.InteropServices.JavaScript.JSType;
using fiap.Repositories;

namespace fiap.Tests.Repositories
{
    public class ClienteRepositoryTests
    {
        private Cliente cliente;
        private List<Cliente> lstCliente = new List<Cliente>();
        public ClienteRepositoryTests()
        {
            cliente = new Cliente { Cpf = "12345678910", Email = "teste@teste.com", Id = 1, Nome = "Joao da Silva" };
            lstCliente = new List<Cliente> {
                new Cliente { Cpf = "12345678910", Email = "teste@teste.com", Id = 1, Nome = "Joao da Silva" } ,
                 new Cliente { Cpf = "22345678910", Email = "teste2@teste.com", Id = 1, Nome = "Joao2 da Silva" }
            };
        }
        //[Fact]
        //public void ObterTest()
        //{
        //    var _repo = new Mock<IDbConnection>();
        //    var _logger = new Mock<Serilog.ILogger>();

        //    var readerMock = new Mock<IDataReader>();

        //    readerMock.SetupSequence(_ => _.Read())
        //        .Returns(true)
        //        .Returns(false);

        //    readerMock.Setup(reader => reader.GetOrdinal("Id")).Returns(0);
        //    readerMock.Setup(reader => reader.GetOrdinal("Name")).Returns(1);

        //    readerMock.Setup(reader => reader.GetInt32(It.IsAny<int>())).Returns(1);
        //    readerMock.Setup(reader => reader.GetString(It.IsAny<int>())).Returns("Hello World");

        //    var commandMock = new Mock<IDbCommand>();
        //    commandMock.Setup(m => m.ExecuteReader()).Returns(readerMock.Object).Verifiable();

        //    var connectionMock = new Mock<IDbConnection>();
        //    connectionMock.Setup(m => m.CreateCommand()).Returns(commandMock.Object);

        //    _repo.Setup(m => m.CreateCommand()).Returns(commandMock.Object);

        //    var data = new ClienteRepository(_logger.Object, _repo.Object);

        //    //Act
        //    var result = data.Obter(1);

           

        //    _repo.SetupSequence(x => x.(It.IsAny<string>()))
        //       .ReturnsAsync(cliente);

        //    ClienteApplication app = new(_logger.Object, _repo.Object);
        //    var result = await app.ObterPorCpf("123");

        //    Assert.NotNull(result);
        //}
    }
}