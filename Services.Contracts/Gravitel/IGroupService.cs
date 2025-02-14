using Entities.Dtos.Gravitel;

namespace Services.Contracts.Gravitel
{
    public interface IGroupService
    {
        Task<IEnumerable<GroupDto>> GetGroups();
    }
}
