using System.Text.Json.Serialization;

namespace Entities.Responses
{
    public record Error : ResponseBase
    {
        [JsonPropertyName("Error")]
        string? ErrorCode { get; init; }
        public string? ErrorDescription { get; init; }
    }
}
