namespace Entities.Dtos.Bitrix
{
    public record ContactDto
    {
        public long Id { get; init; }
        public string? Name { get; init; }
        public required PhoneDto[] Phone { get; init; }
        public long? CompanyId { get; init; }
        public long[]? CompanyIds { get; init; }
    }
}
