namespace Entities.Dtos.Gravitel
{
    public record CallRecordDto
    {
        public string CallId { get; init; }
        public string FileName { get; init; }
        public string? CFileContent { get; init; }
        public string? RecordUrl { get; init; }

        public bool IsValid => (!String.IsNullOrEmpty(CallId)) &&
            (!String.IsNullOrEmpty(FileName));
    }
}
