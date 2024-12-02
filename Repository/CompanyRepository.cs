using Entities.Dtos.Bitrix;
using Entities.Responses;
using OuterSource;
using Repository.Contracts;
using System.Text.Json;

namespace Repository
{
    public class CompanyRepository : RepositoryBase, ICompanyRepository
    {
        public CompanyRepository(Bitrix24Old bitrix) : base(bitrix) 
        {
            
        }

        public Response<CompanyDto>? GetCompany(string id)
        {
            string response = _bitrix.SendCommand("crm.company.get",
                $" ID: {id}");

            return JsonSerializer.Deserialize<Response<CompanyDto>>(response);
        }

        public ListResponse<CompanyDto>? GetCompaniesByFilter(string filter)
        {
            string response = _bitrix.SendCommand("crm.company.list",
                "SELECT: [ 'ID', 'TITLE', 'PHONE' ]," +
                "FILTER: [ " + filter + " ]"
                );

            return JsonSerializer.Deserialize<ListResponse<CompanyDto>>(response);
        }
    }
}
