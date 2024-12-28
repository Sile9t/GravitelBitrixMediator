namespace Entities.Dtos.Gravitel
{
    public record CallRecordDto
    {
        public required string Id { get; init; }
        public long When { get; init; }
        public required string Direction { get; init; }
        public required string Result { get; init; }
        public int Duration { get; init; }
        public int Provision { get; init; }
        public string? Client { get; init; }
        public string? Extension { get; init; }
        public string? Phone { get; init; }
        public string? Record { get; init; }
    }
}
