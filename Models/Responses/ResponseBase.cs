namespace Entities.Responses
{
    public abstract record ResponseBase
    {
        public Error? Error { get; init; }
        public ResponseTime? Time { get; init; }

        public bool IsFailed => (Error is not null);
        public bool IsSuccesful => (Error is null);
    }
}
