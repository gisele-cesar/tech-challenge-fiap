using fiap.Domain.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace fiap.Repositories
{
    public static class RepositoriesModuleDependency
    {
        public static void AddRepositoriesModule(this IServiceCollection services)
        {
            services.AddSingleton<IClienteRepository, ClienteRepository>();
            services.AddSingleton<IProdutoRepository, ProdutoRepository>();
            services.AddSingleton<IPedidoRepository, PedidoRepository>();
        }
    }
}
