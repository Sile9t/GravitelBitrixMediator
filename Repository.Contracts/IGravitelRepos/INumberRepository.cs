using Entities.Dtos.Gravitel;

namespace Repository.Contracts.IGravitelRepos
{
    public interface INumberRepository
    {
        Task<IEnumerable<NumberDto>> GetNumbers();
    }
}
