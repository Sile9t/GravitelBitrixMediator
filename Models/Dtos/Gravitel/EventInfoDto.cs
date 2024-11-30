namespace Entities.Dtos.Gravitel
{
    public record EventInfoDto
    {
        public long Id { get; init; }
        public string? Direction { get; init; }
        public string? Stage { get; init; }
        public string? Client { get; init; }
        public long Extension { get; init; }
        public string? Phone { get; init; }
    }
}
