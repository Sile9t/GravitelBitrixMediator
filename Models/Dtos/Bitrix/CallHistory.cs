namespace Entities.Dtos.Bitrix
{
    public record CallHistory
    {
        public string CallId { get; init; }
        public int Id { get; init; }
        public string RecordFileId { get; init; }
    }
}
