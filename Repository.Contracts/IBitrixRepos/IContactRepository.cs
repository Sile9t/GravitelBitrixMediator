using Entities.Dtos.Bitrix;
using Entities.Responses;

namespace Repository.Contracts.Repos
{
    public interface IContactRepository : IRepositoryBase
    {
        Response<ContactDto>? GetContact(int id);
        ListResponse<ContactDto>? GetContactsByFilter(string filter);
    }
}
