using fiap.Application.Interfaces;
using fiap.Domain.Entities;
using fiap.Domain.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace fiap.Application.UseCases
{
    public class ProdutoApplication : IProdutoApplication
    {
        private readonly ILogger _logger;
        private readonly IProdutoRepository _produtoRepository;
        public ProdutoApplication(ILogger logger, IProdutoRepository produtoRepository)
        {
            _logger = logger;
            _produtoRepository = produtoRepository;
        }
        public async Task<bool> Inserir(Produto produto)
        {
            try
            {
                _logger.Information("Inserindo novo produto.");
                return await _produtoRepository.Inserir(produto);

            }
            catch (Exception ex)
            {
                _logger.Error($"Erro ao incluir produto. Erro: {ex.Message}");
                throw;
            }
        }
        public async Task<List<Produto>> Obter()
        {
            try
            {
                _logger.Information("Buscando lista de produtos.");
                return await _produtoRepository.Obter();
            }
            catch (Exception ex)
            {
                _logger.Error($"Erro ao obter produtos. Erro: {ex.Message}");
                throw;
            }
        }
        public async Task<Produto> Obter(int id)
        {
            try
            {
                _logger.Information($"Buscando produto por id: {id}.");
                return await _produtoRepository.Obter(id);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erro ao obter produto por id {id}. Erro: {ex.Message}");
                throw;
            }
        }
        public async Task<bool> Atualizar(Produto produto)
        {
            try
            {
                _logger.Information($"Atualizando produto id: {produto.IdProduto}");
                return await _produtoRepository.Atualizar(produto);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erro ao atualizar produto id: {produto.IdProduto} cliente. Erro: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Produto>> ObterProdutosPorCategoria(int idCategoriaProduto)
        {
            try
            {
                _logger.Information($"Buscando produtos por categoria idCategoriaProduto: {idCategoriaProduto}.");
                return await _produtoRepository.ObterProdutosPorCategoria(idCategoriaProduto);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erro ao obter produto por categoria id: {idCategoriaProduto}. Erro: {ex.Message}");
                throw;
            }
        }
    }
}
