using Entities.Dtos.Bitrix;
using Repository.Contracts;
using Services.Contracts.Bitrix;

namespace Services.Bitrix
{
    public class CompanyService : ICompanyService
    {
        private readonly IBitrixRepositoryManager _repo;
        private readonly RedisCacheService _cache;

        public CompanyService(IBitrixRepositoryManager repo, RedisCacheService cache)
        {
            _repo = repo;
            _cache = cache;
        }

        public async Task<CompanyDto?> GetCompany(string id)
        {
            var key = this.GetType().Name + "_Company";
            var cachedData = await _cache.GetCachedData<CompanyDto?>(key);

            if (cachedData is null)
            {
                var response = _repo.Company.GetCompany(id);
                var company = (response is not null && response.HasResult) ? response.Result : null;
                await _cache.SetCacheData(key, company, TimeSpan.FromSeconds(60));
                
                return company;
            }

            return cachedData;
        }

        public async Task<IEnumerable<CompanyDto>> GetCompaniesByFilter(string filter)
        {
            var key = this.GetType().Name + "_Company";
            var cachedData = await _cache.GetCachedData<IEnumerable<CompanyDto>>(key);

            if (cachedData is null)
            {
                var response = _repo.Company.GetCompaniesByFilter(filter);
                var companies = (response is not null && response.IsSuccesful) ? response.Result : Enumerable.Empty<CompanyDto>();
                await _cache.SetCacheData(key, companies, TimeSpan.FromSeconds(60));

                return companies!;
            }

            return cachedData;
        }
    }
}
