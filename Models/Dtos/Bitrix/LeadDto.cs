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

    public record LeadForCreationDto
    {
        public long AssignedById { get; set; }
        public long CompanyId { get; set; }
        public string CompanyTitle { get; set; }
        public string ContactId { get; set; }
        public ContactDto[] ContactIds { get; set; }
        public PhoneDto[] Phone { get; set; }
        public string SourceId { get; set; }
        public string StatusId { get; set; }
    }
}
