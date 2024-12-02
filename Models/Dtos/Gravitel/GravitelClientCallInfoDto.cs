namespace Entities.Dtos.Gravitel
{
    public record GravitelClientCallInfoDto
    {
        public required string Id { get; init; }
        public required string ClientPhone { get; init; }
        public required string Phone { get; init; }
    }
}
