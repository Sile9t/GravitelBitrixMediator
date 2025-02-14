using Entities.Dtos.Bitrix;
using Repository.Contracts;
using Services.Contracts.Bitrix;

namespace Services.Bitrix
{
    public class DealService : IDealService
    {
        private readonly IBitrixRepositoryManager _repo;
        private readonly RedisCacheService _cache;
        
        public DealService(IBitrixRepositoryManager repo, RedisCacheService cache)
        {
            _repo = repo;
            _cache = cache;
        }

        public async Task<DealDto?> GetDeal(string id)
        {
            var key = this.GetType().Name + "_Deal_" + id;
            var cachedData = await _cache.GetCachedData<DealDto?>(key);

            if (cachedData is null)
            {
                var response = _repo.Deal.GetDeal(id);
                var deal = (response is not null && response.HasResult) ? response.Result : null;
                await _cache.SetCacheData(key, deal, TimeSpan.FromSeconds(60));

                return deal;
            }

            return cachedData;
        }

        public async Task<IEnumerable<DealDto>> GetDealsByFilter(string filter)
        {
            var key = this.GetType().Name + "_Deals_" + filter;
            var cachedData = await _cache.GetCachedData<IEnumerable<DealDto>>(key);

            if (cachedData is null)
            {
                var response = _repo.Deal.GetDealsByFilter(filter);
                var deals = (response is not null && response.IsSuccesful) ? response.Result : Enumerable.Empty<DealDto>();
                await _cache.SetCacheData(key, deals, TimeSpan.FromSeconds(60));

                return deals!;
            }

            return cachedData;
        }

        public async Task<bool> UpdateDeal(long id, string fields)
        {
            var response = _repo.Deal.UpdateDeal(id, fields);
            var result = (response is not null && response.HasResult) ? response.Result : false;

            return result;
        }
    }
}
