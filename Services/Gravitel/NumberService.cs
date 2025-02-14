using Entities.Dtos.Gravitel;
using Repository.Contracts;
using Repository.Contracts.IGravitelRepos;
using Services.Contracts.Gravitel;

namespace Services.Gravitel
{
    public class NumberService : INumberService
    {
        private readonly IGravitelRepositoryManager _repo;
        private readonly RedisCacheService _cache;

        public NumberService(IGravitelRepositoryManager repo, RedisCacheService cache)
        {
            _repo = repo;
            _cache = cache;
        }

        public async Task<IEnumerable<NumberDto>> GetNumbers()
        {
            var key = this.GetType().Name;
            var cachedData = await _cache.GetCachedData<IEnumerable<NumberDto>>(key);

            if (cachedData is null)
            {
                var numbers = await _repo.Number.GetNumbers();
                await _cache.SetCacheData(key, numbers, TimeSpan.FromSeconds(60));

                return numbers;
            }

            return cachedData;
        }
    }
}
