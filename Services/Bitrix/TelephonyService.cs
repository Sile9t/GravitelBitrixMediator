using Entities.Dtos.Bitrix;
using Repository.Contracts;
using Services.Contracts.Bitrix;

namespace Services.Bitrix
{
    public class TelephonyService : ITelephonyService
    {
        private readonly IBitrixRepositoryManager _repo;
        private readonly RedisCacheService _cache;

        public TelephonyService(IBitrixRepositoryManager repo, RedisCacheService cache)
        {
            _repo = repo;
            _cache = cache;
        }

        public async Task<IEnumerable<CRMEntityDto>> GetCrmEntityByPhone(string phone)
        {
            var key = this.GetType().Name + "_CrmsForNumber_" + phone;
            var cachedData = await _cache.GetCachedData<IEnumerable<CRMEntityDto>>(key);

            if (cachedData is null)
            {
                var response = _repo.Telephony.GetCrmEntityByPhone(phone);
                var crms = (response is not null) ? response : Enumerable.Empty<CRMEntityDto>();
                await _cache.SetCacheData(key, crms, TimeSpan.FromSeconds(60));

                return crms;
            }

            return cachedData;
        }

        public async Task<RegistredDealForCallDto?> RegisterCall(DealForCallDto dealInfo)
        {
            var key = this.GetType().Name + "_Call_" + dealInfo.CallStartDate;
            var cachedData = await _cache.GetCachedData<RegistredDealForCallDto?>(key);

            if (cachedData is null)
            {
                var response = _repo.Telephony.RegisterCall(dealInfo);
                var registredDeal = (response is not null) ? response : null;
                await _cache.SetCacheData(key, registredDeal, TimeSpan.FromSeconds(60));

                return registredDeal;
            }

            return cachedData;
        }

        public async Task ShowCall(string callId, long[] userIds)
        {
            _repo.Telephony.ShowCall(callId, userIds);
        }

        public async Task HideCall(string callId, long[] userIds)
        {
            _repo.Telephony.HideCall(callId, userIds);
        }

        public async Task AttachRecord(string callId, string recordUrl)
        {
            _repo.Telephony.AttachRecord(callId, recordUrl);
        }

        public async Task<IEnumerable<CallHistory>> FinishCall(CallInfoDto callInfo)
        {
            var key = this.GetType().Name + "_FinishCall_" + callInfo.CallId;
            var cachedData = await _cache.GetCachedData<IEnumerable<CallHistory>>(key);

            if (cachedData is null)
            {
                var response = _repo.Telephony.FinishCall(callInfo);
                var contact = (response is not null) ? response : Enumerable.Empty<CallHistory>();
                await _cache.SetCacheData(key, contact, TimeSpan.FromSeconds(60));

                return contact;
            }

            return cachedData;
        }
    }
}
