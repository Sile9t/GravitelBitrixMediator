using System.Text.Json.Serialization;

namespace Entities.Dtos.Gravitel
{
    public record GroupDto
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }
        [JsonPropertyName("extension")]
        public string? Extension { get; init; }
        [JsonPropertyName("name")]
        public string? Name { get; init; }
    }
}
