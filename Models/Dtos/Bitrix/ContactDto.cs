namespace Entities.Dtos.Bitrix
{
    public record ContactDto
    {
        public long Id { get; init; }
        public string? Name { get; init; }
        public PhoneDto[]? Phone { get; init; }
    }
}
