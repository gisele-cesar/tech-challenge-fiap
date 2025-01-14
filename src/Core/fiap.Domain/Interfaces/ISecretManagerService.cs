using System.Threading.Tasks;
using fiap.Domain.Entities;

namespace fiap.Domain.Interfaces
{
    public interface ISecretManagerService
    {
        Task<SecretDbConnect> ObterSecret(string segredo);
    }
}
