namespace Entities.Dtos.Bitrix
{
    public record PhoneDto
    {
        public long Id { get; init; }
        public string? ValueType { get; init; }
        public string? Value { get; init; }
        public string? TypeId { get; init; }
    }
}
