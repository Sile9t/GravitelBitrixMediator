namespace Entities.Dtos.Gravitel
{
    /// <summary>
    /// Record returns from Gravitel /history webhook
    /// </summary>
    public record HistoryRecord
    {

        /// <summary>
        /// Call identifier
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public string Type { get; init; }
        public string Account { get; init; }
        public string Client { get; init; }
        public string Via { get; init; }
        public DateTime Start { get; init; }
        public int Wait { get; init; }
        public int Duraiton { get; init; }
        public string Record { get; init; }
    }
}
