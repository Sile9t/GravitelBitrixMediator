using Entities.Dtos.Bitrix;

namespace Services.Contracts.Bitrix
{
    public interface IDealService
    {
        Task<DealDto?> GetDeal(string id);
        Task<IEnumerable<DealDto>> GetDealsByFilter(string filter);
        Task<bool> UpdateDeal(long id, string fields);
    }
}
