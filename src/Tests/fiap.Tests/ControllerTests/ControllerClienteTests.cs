using fiap.API.Controllers;
using fiap.Application.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fiap.Tests.ControllerTests
{
    public class ControllerClienteTests
    {
        
        [Fact]
        public async Task Get_OKAsync()
        {
            var _clienteApplication = new Mock<IClienteApplication>();
            var _logger = new Mock<Serilog.ILogger>();

            _clienteApplication.Setup(x => x.Obter(It.IsAny<int>())).ReturnsAsync(new Domain.Entities.Cliente { Cpf = "12345678910" , Email = "teste@teste.com", Id = 1, Nome = "Joao da Silva" });

            ClienteController clienteController = new(_logger.Object , _clienteApplication.Object);

            var result = await clienteController.Get();

            Assert.NotNull(result);
        }
    }
}
