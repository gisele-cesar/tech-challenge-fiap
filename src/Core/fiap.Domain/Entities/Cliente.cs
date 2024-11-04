using System.Text.Json.Serialization;

namespace fiap.Domain.Entities
{
    public class Cliente
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
    }
}
