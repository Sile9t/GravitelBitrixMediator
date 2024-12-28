using Entities.Dtos.Bitrix;
using Entities.Responses;
using OuterSource;
using Repository.Contracts.Repos;
using System.Text.Json;

namespace Repository.Repos
{
    public class DealRepository : RepositoryBase, IDealRepository
    {
        public DealRepository(Bitrix24Old bitrix) : base(bitrix)
        {
            
        }

        public Response<DealDto>? GetDeal(string id)
        {
            string response = _bitrix.SendCommand("crm.deal.get",
                $" ID: {id}");

            return JsonSerializer.Deserialize<Response<DealDto>>(response);
        }

        public ListResponse<DealDto>? GetDealsByFilter(string filter)
        {
            string response = _bitrix.SendCommand("crm.deal.list",
                "SELECT: [ 'ID', 'ASSIGNED_BY_ID', 'ASSIGNED_BY', 'LEAD_ID', 'LEAD_BY', 'COMPANY_ID',"
                    + "'CONTACT_ID', 'CONTACT_IDS' ]," +
                "FILTER: [ " + filter + " ]"
                );

            return JsonSerializer.Deserialize<ListResponse<DealDto>>(response);
        }

        public Response<bool>? UpdateDeal(long id, string fields)
        {
            string response = _bitrix.SendCommand("crm.deal.update",
                $"ID: {id}," +
                $"'FIELDS => [ {fields} ]");

            return JsonSerializer.Deserialize<Response<bool>>(response);
        }
    }
}
