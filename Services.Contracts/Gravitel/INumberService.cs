using Entities.Dtos.Gravitel;

namespace Services.Contracts.Gravitel
{
    public interface INumberService
    {
        Task<IEnumerable<NumberDto>> GetNumbers();
    }
}
