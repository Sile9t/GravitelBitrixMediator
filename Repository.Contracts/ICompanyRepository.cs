using Entities.Dtos.Bitrix;
using Entities.Responses;

namespace Repository.Contracts
{
    public interface ICompanyRepository : IRepositoryBase
    {
        Response<CompanyDto>? GetCompany(string id);
        ListResponse<CompanyDto>? GetCompaniesByFilter(string filter);
    }
}