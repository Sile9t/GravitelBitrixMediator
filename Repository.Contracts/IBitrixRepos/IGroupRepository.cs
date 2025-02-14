using Entities.Dtos.Bitrix;
using Entities.Responses;

namespace Repository.Contracts.Repos
{
    public interface IGroupRepository
    {
        ListResponse<GroupDto>? GetGroupsByFilter(string filter);
    }
}
