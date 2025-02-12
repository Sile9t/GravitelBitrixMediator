using System.Text.Json.Serialization;

namespace Entities.Dtos.Gravitel
{
    public record AccountDto
    {
        [JsonPropertyName("extension")]
        public string? Extension { get; init; }
        [JsonPropertyName("name")]
        public string? Name { get; init; }
    }
}
