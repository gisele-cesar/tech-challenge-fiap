using System.Threading.Tasks;

namespace fiap.Domain.Services.Interfaces
{
    public interface ISecretManagerService
    {
        Task<SecretDbConnect> ObterSecret(string segredo);
    }
}
