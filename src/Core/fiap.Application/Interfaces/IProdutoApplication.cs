using fiap.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace fiap.Application.Interfaces
{
    public interface IProdutoApplication
    {
        Task<Produto> Obter(int id);
        Task<List<Produto>> Obter();
        Task<bool> Inserir(Produto cliente);
        Task<bool> Atualizar(Produto cliente);
        Task<List<Produto>> ObterProdutosPorCategoria(int idCategoriaProduto);
    }
}
