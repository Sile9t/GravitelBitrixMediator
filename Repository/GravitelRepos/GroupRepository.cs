using Entities.Dtos.Gravitel;
using OuterSource;
using Repository.Contracts.IGravitelRepos;

namespace Repository.GravitelRepos
{
    public class GroupRepository : GravitelRepositoryBase, IGroupRepository
    {
        public GroupRepository(Gravitel gravitel) : base(gravitel)
        {
            
        }

        public async Task<IEnumerable<GroupDto>> GetGroups()
        {
            var response = await _gravitel.SendCommand<List<GroupDto>>(HttpMethod.Get, "groups");

            if (response is null)
                return Enumerable.Empty<GroupDto>();

            return response;
        }
    }
}
