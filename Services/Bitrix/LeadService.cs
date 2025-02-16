using Entities.Dtos.Bitrix;
using Repository.Contracts;
using Services.Contracts.Bitrix;

namespace Services.Bitrix
{
    public class LeadService : ILeadService
    {
        private readonly IBitrixRepositoryManager _repo;
        private readonly RedisCacheService _cache;

        public LeadService(IBitrixRepositoryManager repo, RedisCacheService cache)
        {
            _repo = repo;
            _cache = cache;
        }

        public async Task<LeadDto?> GetLead(string id)
        {
            var key = this.GetType().Name + "_Lead_" + id;
            var cachedData = await _cache.GetCachedData<LeadDto?>(key);

            if (cachedData is null)
            {
                var response = _repo.Lead.GetLead(id);
                var lead = (response is not null && response.HasResult) ? response.Result : null;
                await _cache.SetCacheData(key, lead, TimeSpan.FromSeconds(60));

                return lead;
            }

            return cachedData;
        }

        public async Task<IEnumerable<LeadDto>> GetLeadsByFilter(string filter)
        {
            var key = this.GetType().Name + "_Leads_" + filter;
            var cachedData = await _cache.GetCachedData<IEnumerable<LeadDto>>(key);

            if (cachedData is null)
            {
                var response = _repo.Lead.GetLeadsByFilter(filter);
                IEnumerable<LeadDto> leads = (response is not null && response.IsSuccesful) ? response.Result! : [];
                await _cache.SetCacheData(key, leads, TimeSpan.FromSeconds(60));

                return leads;
            }

            return cachedData;
        }

        public async Task<string?> CreateLead(LeadForCreationDto leadForCreation)
        {
            var response = _repo.Lead.CreateLead(leadForCreation);

            return (response is not null && response.HasResult) ? response.Result : null;
        }
    }
}
