using Entities.Dtos.Bitrix;
using Entities.Responses;
using OuterSource;
using Repository.Contracts.Repos;
using System.Text.Json;

namespace Repository.Repos
{
    public class GroupRepository : RepositoryBase, IGroupRepository
    {
        public GroupRepository(Bitrix24Old bitrix) : base(bitrix)
        {
            
        }

        public Response<GroupDto>? GetGroup(string id)
        {
            string response = _bitrix.SendCommand("department.get",
                $" ID: {id}");

            return JsonSerializer.Deserialize<Response<GroupDto>>(response);
        } 
    }
}
