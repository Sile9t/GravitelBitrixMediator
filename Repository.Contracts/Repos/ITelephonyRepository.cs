using Entities.Dtos.Bitrix;

namespace Repository.Contracts.Repos
{
    public interface ITelephonyRepository : IRepositoryBase
    {
        CRMEntityDto[]? GetCrmEntityByPhone(string phone);
        RegistredDealForCallDto? RegisterCall(DealForCallDto dealInfo);
        void ShowCall(string CallId, long[] UserId);
        void HideCall(string CallId, long[] UserId);
        CallHistory[]? FinishCall(CallInfoDto callInfo);
        void AttachRecord(string CallId, string ResordUrl);
    }
}
