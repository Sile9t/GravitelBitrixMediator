namespace Entities.Dtos.Gravitel
{

    /// <summary>
    /// Record returns from Gravitel /history webhook, after call is ended
    /// </summary>
    public record CallRecordDto
    {

        /// <summary>
        /// Call identifier, unique
        /// </summary>
        public required string Id { get; init; }
        
        /// <summary>
        /// Time when call is started
        /// </summary>
        public long When { get; init; }

        /// <summary>
        /// Direction of call, can be "in" or "out"
        /// </summary>
        public required string Direction { get; init; }
        
        /// <summary>
        /// Call result, can be { success, missed, not_available, forbidden, busy, unknown }
        /// </summary>
        public required string Result { get; init; }

        /// <summary>
        /// Call duration in seconds
        /// </summary>
        public int Duration { get; init; }

        /// <summary>
        /// Call provision in seconds
        /// </summary>
        public int Provision { get; init; }

        /// <summary>
        /// Client phone number
        /// </summary>
        public string? Client { get; init; }

        /// <summary>
        /// Inner number of user in Gravitel
        /// </summary>
        public string? Extension { get; init; }

        /// <summary>
        /// Phone number on what client has called, or (if it's out call) from what it's done
        /// </summary>
        public string? Phone { get; init; }

        /// <summary>
        /// Link to call record
        /// </summary>
        public string? Record { get; init; }
    }
}
