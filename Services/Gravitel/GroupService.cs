using Entities.Dtos.Gravitel;
using Repository.Contracts;
using Services.Contracts.Gravitel;

namespace Services.Gravitel
{
    public class GroupService : IGroupService
    {
        private readonly IGravitelRepositoryManager _repo;
        private readonly RedisCacheService _cache;

        public GroupService(IGravitelRepositoryManager repo, RedisCacheService cache)
        {
            _repo = repo;
            _cache = cache;
        }

        public async Task<IEnumerable<GroupDto>> GetGroups()
        {
            var key = this.GetType().Name;
            var cachedData = await _cache.GetCachedData<IEnumerable<GroupDto>>(key);

            if (cachedData is null)
            {
                var groups = await _repo.Group.GetGroups();
                await _cache.SetCacheData(key, groups, TimeSpan.FromSeconds(60));

                return groups;
            }

            return cachedData;
        }
    }
}
