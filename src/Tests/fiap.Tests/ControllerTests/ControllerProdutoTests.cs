using fiap.API.Controllers;
using fiap.Application.DTO;
using fiap.Application.Interfaces;
using fiap.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fiap.Tests.ControllerTests
{
    public class ControllerProdutoTests
    {
        private readonly Produto produto;
        private readonly List<Produto> lstProduto = [];
        public ControllerProdutoTests()
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
        public async Task Get_OKAsync()
        {
            var _application = new Mock<IProdutoApplication>();
            var _pagamentoapplication = new Mock<IPagamentoApplication>();
            var _logger = new Mock<Serilog.ILogger>();
            _application.SetupSequence(x => x.Obter()).ReturnsAsync(lstProduto);

            ProdutoController controller = new(_logger.Object , _application.Object);
            var result = await controller.Get();

            Assert.NotNull(result);
        }
        [Fact]
        public async Task Get_PorId_OKAsync()
        {
            var _application = new Mock<IProdutoApplication>();
            var _pagamentoapplication = new Mock<IPagamentoApplication>();
            var _logger = new Mock<Serilog.ILogger>();
            _application.SetupSequence(x => x.Obter(1)).ReturnsAsync(produto);

            ProdutoController controller = new(_logger.Object, _application.Object);
            var result = await controller.Get(1);

            Assert.NotNull(result);
        }
       
        [Fact]
        public async Task POST_OKAsync()
        {
            var _application = new Mock<IProdutoApplication>();
            var _pagamentoapplication = new Mock<IPagamentoApplication>();
            var _logger = new Mock<Serilog.ILogger>();
            _application.SetupSequence(x => x.Inserir(produto)).ReturnsAsync(true);

            ProdutoController controller = new(_logger.Object, _application.Object);
            var result = await controller.Post(produto);

            Assert.NotNull(result);
        }
        [Fact]
        public async Task PUT_OKAsync()
        {
            var _application = new Mock<IProdutoApplication>();
            var _pagamentoapplication = new Mock<IPagamentoApplication>();
            var _logger = new Mock<Serilog.ILogger>();
            _application.SetupSequence(x => x.Atualizar(produto)).ReturnsAsync(true);

            ProdutoController controller = new(_logger.Object, _application.Object);
            var result = await controller.Put(produto);

            Assert.NotNull(result);
        }
    }
}
