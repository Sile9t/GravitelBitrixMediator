namespace Entities.Dtos.Bitrix
{
    public record LeadDto
    {
        public long Id { get; init; }
        public long? CompanyId { get; init; }
        public string? CompanyTitle { get; init; }
        public long? ContactId { get; init; }
        public long AssignedById { get; init; }
        public PhoneDto[]? Phone { get; init; }
    }
}
