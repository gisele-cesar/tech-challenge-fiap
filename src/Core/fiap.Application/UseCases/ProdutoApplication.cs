using fiap.Application.Interfaces;
using fiap.Domain.Entities;
using fiap.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace fiap.Application.UseCases
{
    public class ProdutoApplication : IProdutoApplication
    {
        private readonly IProdutoRepository _produtoRepository;
        public ProdutoApplication(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }
        public async Task<bool> Inserir(Produto produto)
        {
            return await _produtoRepository.Inserir(produto);
        }
        public async Task<List<Produto>> Obter()
        {

            return await _produtoRepository.Obter();
        }
        public async Task<Produto> Obter(int id)
        {

            return await _produtoRepository.Obter(id);
        }
        public async Task<bool> Atualizar(Produto produto)
        {
            return await _produtoRepository.Atualizar(produto);
        }

        public async Task<List<Produto>> ObterProdutosPorCategoria(int idCategoriaProduto)
        {
            return await _produtoRepository.ObterProdutosPorCategoria(idCategoriaProduto);
        }
    }
}
