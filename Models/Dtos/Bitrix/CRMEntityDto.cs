namespace Entities.Dtos.Bitrix
{
    public record CRMEntityDto
    {
        public string? CRMEntityType { get; init; }
        public long CRMEntityId { get; init; }
        public long? AssignedById { get; init; }
        public AssignedEntityDto[] AssignedBy { get; init; } = Array.Empty<AssignedEntityDto>();
    }
}
