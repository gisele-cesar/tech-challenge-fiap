using fiap.Application.UseCases;
using fiap.Domain.Entities;
using fiap.Domain.Interfaces;
using Moq;
using Xunit;

namespace fiap.Tests.Application
{
    public class ProdutoApplicationTests
    {
        private readonly Produto produto;
        private readonly List<Produto> lstProduto = [];
        public ProdutoApplicationTests()
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
        public async Task InserirProduto_OkAsync()
        {
            var _repo = new Mock<IProdutoRepository>();
            var _logger = new Mock<Serilog.ILogger>();

            _repo.SetupSequence(x => x.Inserir(produto))
                .ReturnsAsync(true);

            ProdutoApplication app = new(_logger.Object, _repo.Object);
            var result = await app.Inserir(produto);

            Assert.True(result);
        }
        [Fact]
        public async Task Obter_OkAsync()
        {
            var _repo = new Mock<IProdutoRepository>();
            var _logger = new Mock<Serilog.ILogger>();

            _repo.SetupSequence(x => x.Obter())
                .ReturnsAsync(lstProduto);

            ProdutoApplication app = new(_logger.Object, _repo.Object);
            var result = await app.Obter();

            Assert.NotNull(result);
        }
        [Fact]
        public async Task ObterProdutosPorId_OkAsync()
        {
            var _repo = new Mock<IProdutoRepository>();
            var _logger = new Mock<Serilog.ILogger>();

            _repo.SetupSequence(x => x.Obter(1))
                .ReturnsAsync(produto);

            ProdutoApplication app = new(_logger.Object, _repo.Object);
            var result = await app.Obter(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task ObterProdutosPorCategoria_OkAsync()
        {
            var _repo = new Mock<IProdutoRepository>();
            var _logger = new Mock<Serilog.ILogger>();

            _repo.SetupSequence(x => x.ObterProdutosPorCategoria(1))
                .ReturnsAsync(lstProduto);

            ProdutoApplication app = new(_logger.Object, _repo.Object);
            var result = await app.ObterProdutosPorCategoria(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Atualizar_OkAsync()
        {
            var _repo = new Mock<IProdutoRepository>();
            var _logger = new Mock<Serilog.ILogger>();

            _repo.SetupSequence(x => x.Atualizar(produto))
                .ReturnsAsync(true);

            ProdutoApplication app = new(_logger.Object, _repo.Object);
            var result = await app.Atualizar(produto);

            Assert.True(result);
        }
    }
}
