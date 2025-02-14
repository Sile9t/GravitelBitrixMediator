using Entities.Dtos.Bitrix;

namespace Services.Contracts.Bitrix
{
    public interface IGroupService
    {
        Task<IEnumerable<GroupDto>> GetGroupsByFilter(string filter);
    }
}
