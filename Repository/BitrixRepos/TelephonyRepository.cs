using Entities.Dtos.Bitrix;
using OuterSource;
using Repository.Contracts.Repos;
using System.Numerics;
using System.Text;
using System.Text.Json;

namespace Repository.Repos
{
    public class TelephonyRepository : BitrixRepositoryBase, ITelephonyRepository
    {
        public TelephonyRepository(Bitrix24Old bitrix) : base(bitrix)
        {

        }

        public CRMEntityDto[]? GetCrmEntityByPhone(string phone)
        {
            string response = _bitrix.SendCommand("telephony.externalCall.searchCrmEntities",
                $"'PHONE_NUMBER'=>{phone}");

            return JsonSerializer.Deserialize<CRMEntityDto[]>(response);
        }

        public RegistredDealForCallDto? RegisterCall(DealForCallDto dealInfo)
        {
            string body = JsonSerializer.Serialize(dealInfo);
            string response = _bitrix.SendCommand("telephony.externalCall.register",
                Body: body);

            return JsonSerializer.Deserialize<RegistredDealForCallDto>(response);
        }

        public void ShowCall(string CallId, long[] UserId)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < UserId.Length; i++)
            {
                builder.Append($"USER_ID[{i}]={UserId[i]}");
                if (i < UserId.Length - 1)
                    builder.Append("&");
            }

            string response = _bitrix.SendCommand("telephony.externalCall.show",
                $"'CALL_ID'=>{CallId}, {builder.ToString()}");
            builder = null;
        }
        
        public void HideCall(string CallId, long[] UserId)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < UserId.Length; i++)
            {
                builder.Append($"USER_ID[{i}]={UserId[i]}");
                if (i < UserId.Length - 1)
                    builder.Append("&");
            }

            string response = _bitrix.SendCommand("telephony.externalCall.hide",
                $"'CALL_ID'=>{CallId}, {builder.ToString()}");
            builder = null;
        }

        public CallHistory[]? FinishCall(CallInfoDto callInfo)
        {
            string body = JsonSerializer.Serialize(callInfo);
            string response = _bitrix.SendCommand("telephony.exteranalCall.finish",
                Body: body);

            return JsonSerializer.Deserialize<CallHistory[]>(response);
        }

        public void AttachRecord(string CallId, string RecordUrl)
        {
            string response = _bitrix.SendCommand("telephony.externalCall.hide",
                $"CALL_ID=>'{CallId}', 'FILENAME'='{CallId}.mp3','RECORD_URL'='{RecordUrl}'");
        }
    }
}
