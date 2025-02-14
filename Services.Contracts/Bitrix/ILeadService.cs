using Entities.Dtos.Bitrix;

namespace Services.Contracts.Bitrix
{
    public interface ILeadService
    {
        Task<LeadDto?> GetLead(string id);
        Task<IEnumerable<LeadDto>> GetLeadsByFilter(string filter);
        Task<string?> CreateLead(LeadForCreationDto leadForCreation);
    }
}
