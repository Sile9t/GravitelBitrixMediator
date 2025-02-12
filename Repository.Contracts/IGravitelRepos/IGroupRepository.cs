using Entities.Dtos.Gravitel;

namespace Repository.Contracts.IGravitelRepos
{
    public interface IGroupRepository
    {
        Task<List<GroupDto>?> GetGroups();
    }
}
