using Entities.Dtos.Bitrix;
using Repository.Contracts;
using Services.Contracts.Bitrix;

namespace Services.Bitrix
{
    public class GroupService : IGroupService
    {
        private readonly IBitrixRepositoryManager _repo;
        private readonly RedisCacheService _cache;
        
        public GroupService(IBitrixRepositoryManager repo, RedisCacheService cache)
        {
            _repo = repo;
            _cache = cache;
        }
        
        public async Task<IEnumerable<GroupDto>> GetGroupsByFilter(string filter)
        {
            var key = this.GetType().Name + "_Groups_" + filter;
            var cachedData = await _cache.GetCachedData<IEnumerable<GroupDto>>(key);

            if (cachedData is null)
            {
                var response = _repo.Group.GetGroupsByFilter(filter);
                var groups = (response is not null && response.IsSuccesful) ? response.Result : Enumerable.Empty<GroupDto>();
                await _cache.SetCacheData(key, groups, TimeSpan.FromSeconds(60));

                return groups!;
            }

            return cachedData;
        }
    }
}
