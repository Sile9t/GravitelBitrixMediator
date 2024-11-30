namespace Entities.Dtos.Bitrix
{
    public record CallInfoDto
    {
        public string CallId { get; init; }
        public string? UserPhoneInner { get; init; }
        public int? UserId { get; init; }
        public int Duration { get; init; } = -1;
        public double? Cost{ get; init; }
        public string? CostCurrency { get; init; }
        public string? StatusCode { get; init; }
        public string? FailedReason{ get; init; }
        public string? RecordUrl{ get; init; }
        public int? Vote { get; init; }
        public int? AddToChat { get; init; }

        public bool DataIsValid => ((UserPhoneInner is not null) || (UserId is null)) &&
            (!String.IsNullOrEmpty(CallId)) &&
            (Duration > -1);
    }
}
