using System.Text.Json.Serialization;

namespace fiap.Domain.Entities
{
    public class SecretMercadoPago
    {
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }
        [JsonPropertyName("userId")]
        public string UserId { get; set; }
        [JsonPropertyName("externalPosId")]
        public string ExternalPosId { get; set; }
    }
}
