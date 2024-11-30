namespace Entities.Dtos.Bitrix
{
    public record CompanyDto
    {
        public long Id { get; init; }
        public string? Title { get; init; }
        public PhoneDto[]? Phone { get; init; }
    }
}
