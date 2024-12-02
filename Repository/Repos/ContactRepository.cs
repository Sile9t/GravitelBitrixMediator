using Entities.Dtos.Bitrix;
using Entities.Responses;
using OuterSource;
using Repository.Contracts.Repos;
using System.Text.Json;

namespace Repository.Repos
{
    public class ContactRepository : RepositoryBase, IContactRepository
    {
        public ContactRepository(Bitrix24Old bitrix) : base(bitrix)
        {
            
        }

        public Response<ContactDto>? GetContact(int id)
        {
            string response = _bitrix.SendCommand("crm.contact.get",
                $" ID: {id}");

            return JsonSerializer.Deserialize<Response<ContactDto>>(response);
        }

        public ListResponse<ContactDto>? GetContactsByFilter(string filter)
        {
            string response = _bitrix.SendCommand("crm.contact.list",
                "SELECT: [ 'ID', 'NAME', 'PHONE' ]," +
                "FILTER: [ " + filter + "]"
                );

            return JsonSerializer.Deserialize<ListResponse<ContactDto>>(response);
        }
    }
}
