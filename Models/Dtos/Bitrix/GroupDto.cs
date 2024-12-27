namespace Entities.Dtos.Bitrix
{
    public record GroupDto
    {
        public int Id { get; init; }
        public string? Name { get; init; }
        public int ParentId { get; init; }
        public int HeadId { get; init; }
    }
}
