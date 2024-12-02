namespace Entities.Dtos.Bitrix
{
    public record ContactDto
    {
        public long Id { get; init; }
        public string? Name { get; init; }
        public required PhoneDto[] Phone { get; init; }
    }
}
