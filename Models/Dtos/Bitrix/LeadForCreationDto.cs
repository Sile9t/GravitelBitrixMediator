namespace Entities.Dtos.Bitrix
{
    public record LeadForCreationDto
    {
        public long AssignedById { get; set;}
        public long CompanyId { get; set;}
        public string CompanyTitle { get; set;}
        public string ContactId { get; set;}
        public ContactDto[] ContactIds { get; set;}
        public PhoneDto[] Phone { get; set;}
        public string SourceId { get; set;}
        public string StatusId { get; set;}
    }
}
