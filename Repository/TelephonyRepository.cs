using Entities.Dtos.Bitrix;
using OuterSource;
using Repository.Contracts;
using System.Text.Json;

namespace Repository
{
    public class TelephonyRepository : RepositoryBase, ITelephonyRepository
    {
        public TelephonyRepository(Bitrix24Old bitrix) : base(bitrix)
        {
            
        }

        public CRMEntityDto[]? GetCrmEntityByPhone(string phone)
        {
            string response = _bitrix.SendCommand("telephony.externalCall.searchCrmEntities",
                $"PHONE_NUMBER={phone}");

            return JsonSerializer.Deserialize<CRMEntityDto[]>(response);
        }

        public RegistredDealForCallDto? RegisterCall(DealForCallDto dealInfo)
        {
            string body = JsonSerializer.Serialize(dealInfo);
            string response = _bitrix.SendCommand("telephony.externalCall.register",
                Body: body);

            return JsonSerializer.Deserialize<RegistredDealForCallDto>(response);
        }

        public void ShowCall(string CallId, int[] UserId)
        {
            ShowCall(CallId, UserId);
        }

        public CallHistory[]? FinishCall(CallInfoDto callInfo)
        {
            string body = JsonSerializer.Serialize(callInfo);
            string response = _bitrix.SendCommand("telephony.exteranalCall.finish",
                Body: body);

            return JsonSerializer.Deserialize<CallHistory[]>(response);
        }
    }
}
