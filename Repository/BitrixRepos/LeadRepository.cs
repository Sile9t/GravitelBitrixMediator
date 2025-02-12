using Entities.Dtos.Bitrix;
using Entities.Responses;
using OuterSource;
using Repository.Contracts.Repos;
using System.Text.Json;

namespace Repository.Repos
{
    public class LeadRepository : BitrixRepositoryBase, ILeadRepository
    {
        public LeadRepository(Bitrix24Old bitrix) : base(bitrix)
        {
            
        }

        public Response<LeadDto>? GetLead(string id)
        {
            string response = _bitrix.SendCommand("crm.lead.get",
                $" ID: {id}");

            return JsonSerializer.Deserialize<Response<LeadDto>>(response);
        }

        public ListResponse<LeadDto>? GetLeadsByFilter(string filter)
        {
            string response = _bitrix.SendCommand("crm.lead.list",
                "SELECT: [ 'ID', 'COMPANY_ID', 'COMPANY_TITLE', 'CONTACT_ID', 'ASSIGNED_BY_ID', 'PHONE' ]," +
                "FILTER: [ " + filter + " ]"
                );

            return JsonSerializer.Deserialize<ListResponse<LeadDto>>(response);
        }

        public Response<string>? CreateLead(LeadForCreationDto leadForCreation)
        {
            string body = JsonSerializer.Serialize(leadForCreation);
            string response = _bitrix.SendCommand("crm.lead.add",
                Body: body);

            return JsonSerializer.Deserialize<Response<string>>(response);
        }
    }
}
