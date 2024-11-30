namespace Entities.Dtos.Bitrix
{
    public record AssignedEntityDto
    {
        public long Id { get; init; }
        public string? TimemanStatus { get; init; }
        public string UserPhoneInner { get; init; }
        public string? WorkPhone { get; init; }
        public string? PersonalPhone { get; init; }
        public string? PersonalMobile { get; init; }
    }
}
