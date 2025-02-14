using Entities.Dtos.Bitrix;
using Entities.Responses;
using OuterSource;
using Repository.Contracts.Repos;
using System.Text.Json;

namespace Repository.Repos
{
    public class GroupRepository : BitrixRepositoryBase, IGroupRepository
    {
        public GroupRepository(Bitrix24Old bitrix) : base(bitrix)
        {
            
        }

        public ListResponse<GroupDto>? GetGroupsByFilter(string filter)
        {
            string response = _bitrix.SendCommand("department.get",
                $"FILTER: [ " + filter + "]");

            return JsonSerializer.Deserialize<ListResponse<GroupDto>>(response);
        } 
    }
}
