using System.Text.Json.Serialization;

namespace Entities.Dtos.Gravitel
{
    public record NumberDto
    {
        [JsonPropertyName("number")]
        public string? Number { get; init; }
        [JsonPropertyName("name")]
        public string? Name { get; init; }
    }
}
