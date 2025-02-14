using Entities.Dtos.Gravitel;

namespace Repository.Contracts.IGravitelRepos
{
    public interface IGroupRepository
    {
        Task<IEnumerable<GroupDto>> GetGroups();
    }
}
