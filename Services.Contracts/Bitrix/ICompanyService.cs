using Entities.Dtos.Bitrix;
using Entities.Responses;

namespace Services.Contracts.Bitrix
{
    public interface ICompanyService
    {
        Task<CompanyDto?> GetCompany(string id);
        Task<IEnumerable<CompanyDto>> GetCompaniesByFilter(string filter);

    }
}
