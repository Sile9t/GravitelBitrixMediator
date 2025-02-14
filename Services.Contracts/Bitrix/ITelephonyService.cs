using Entities.Dtos.Bitrix;

namespace Services.Contracts.Bitrix
{
    public interface ITelephonyService
    {
        Task<IEnumerable<CRMEntityDto>> GetCrmEntityByPhone(string phone);
        Task<RegistredDealForCallDto?> RegisterCall(DealForCallDto dealInfo);
        Task ShowCall(string callId, long[] userIds);
        Task HideCall(string callId, long[] userIds);
        Task<IEnumerable<CallHistory>> FinishCall(CallInfoDto callInfo);
        Task AttachRecord(string callId, string recordUrl);
    }
}
