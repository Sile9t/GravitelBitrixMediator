using Entities.Dtos.Gravitel;
using Repository.Contracts;
using Repository.Contracts.IGravitelRepos;
using Repository.Contracts.Repos;
using Services.Contracts.Gravitel;

namespace Services.Gravitel
{
    public class AccountService : IAccountService
    {
        private readonly IGravitelRepositoryManager _repo;
        private readonly RedisCacheService _cache;

        public AccountService(IGravitelRepositoryManager repo, RedisCacheService cache)
        {
            _repo = repo;
            _cache = cache;
        }

        public async Task<IEnumerable<AccountDto>> GetAccounts()
        {
            var key = this.GetType().Name;
            var cachedData = await _cache.GetCachedData<IEnumerable<AccountDto>>(key);

            if (cachedData is null)
            {
                var accounts = await _repo.Account.GetAccounts();
                await _cache.SetCacheData(key, accounts, TimeSpan.FromSeconds(60));
                
                return accounts;
            }

            return cachedData;
        }
    }
}
