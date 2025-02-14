using Entities.Dtos.Bitrix;
using Entities.Responses;

namespace Repository.Contracts.Repos
{
    public interface ILeadRepository : IRepositoryBase
    {
        Response<LeadDto>? GetLead(string id);
        ListResponse<LeadDto>? GetLeadsByFilter(string filter);
        Response<string>? CreateLead(LeadForCreationDto leadForCreation);
    }
}
