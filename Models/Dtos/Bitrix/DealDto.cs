namespace Entities.Dtos.Bitrix
{
    public record DealDto
    {
        public long Id { get; init; }
        public long? AssignedById { get; init; }
        public long? LeadId { get; init; }
        public long? CompanyId { get; init; }
        public long? ContactId { get; init; }
        public long[]? ContactIds { get; init; }
    }

    public record DealForCallDto
    {
        public string? UserPhoneInner { get; init; } = null;
        public int? UserId { get; init; } = null;
        public string PhoneNumber { get; init; }
        public string? CallStartDate {  get; init; }
        public string? CrmCreate { get; init; }
        public string? CrmEntityType { get; init; }
        public int CrmEntityId { get; init; }
        public int Show { get; init; }
        public int CallListId { get; init; }
        public string LineNumber { get; init; }
        public int Type { get; init; }

        public bool DataIsValid => ((UserPhoneInner is not null) || (UserId is not null)) &&
            (!String.IsNullOrEmpty(PhoneNumber)) &&
            (!String.IsNullOrEmpty(LineNumber)) &&
            ((Type > 0) && (Type < 5)); 
    }

    public record RegistredDealForCallDto
    {
        public string? CallId { get; init; }
        public int CrmCreatedLead { get; init; }
        public int CrmEntityId { get; init; }
        public string? CrmEntityType { get; init; }
        public CRMEntityDto[] CrmCreatedEntities { get; init; }
        public string? LeadCreationError { get; init; }

        public bool IsSuccessful => (String.IsNullOrEmpty(LeadCreationError));
        public bool IsFailed => !IsSuccessful;
    }
}
