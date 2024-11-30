namespace Entities.Responses
{
    public record Response<T> : ResponseBase
    {
        public T? Result { get; init; }

        public bool HasResult => (Result is not null);
    }
}
