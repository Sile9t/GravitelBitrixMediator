namespace Entities.Responses
{
    public record ListResponse<T> : ResponseBase
    {
        public T[]? Result { get; init; }
        public long Total { get; init; }
    }
}
