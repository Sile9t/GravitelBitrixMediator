using Entities.Dtos.Bitrix;
using Entities.Responses;

namespace Repository.Contracts.Repos
{
    public interface IGroupRepository
    {
        Response<GroupDto>? GetGroup(string id);
    }
}
