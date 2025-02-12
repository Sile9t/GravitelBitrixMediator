using Entities.Dtos.Bitrix;
using Entities.Responses;

namespace Repository.Contracts.Repos
{
    public interface IDealRepository : IRepositoryBase
    {
        Response<DealDto>? GetDeal(string id);
        ListResponse<DealDto>? GetDealsByFilter(string filter);
        Response<bool>? UpdateDeal(long id, string fields);
    }
}
