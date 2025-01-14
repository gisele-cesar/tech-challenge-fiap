using fiap.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace fiap.Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task<Produto> Obter(int id);
        Task<List<Produto>> Obter();
        Task<bool> Inserir(Produto produto);
        Task<bool> Atualizar(Produto produto);
        Task<List<Produto>> ObterProdutosPorCategoria(int idCategoriaProduto);
    }
}
