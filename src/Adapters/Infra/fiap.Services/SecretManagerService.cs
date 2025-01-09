using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using fiap.Domain.Services;
using fiap.Domain.Services.Interfaces;
using System.Text.Json;

namespace fiap.Services
{
    public class SecretManagerService : ISecretManagerService
    {
        private readonly IAmazonSecretsManager _secret;
        public SecretManagerService(IAmazonSecretsManager secret)
        {
            _secret = secret;
        }
        public async Task<SecretDbConnect> ObterSecret(string segredo)
        {
            GetSecretValueRequest request = new GetSecretValueRequest
            {
                SecretId = segredo,
                VersionStage = "AWSCURRENT"
            };

            GetSecretValueResponse response;

            try
            {
                response = await _secret.GetSecretValueAsync(request);
            }
            catch (Exception)
            {
                throw;
            }

            return JsonSerializer.Deserialize<SecretDbConnect>(response.SecretString);
        }
    }
}
