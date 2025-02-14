using Entities.Dtos.Gravitel;
using OuterSource;
using Repository.Contracts.IGravitelRepos;

namespace Repository.GravitelRepos
{
    public class NumberRepository : GravitelRepositoryBase, INumberRepository
    {
        public NumberRepository(Gravitel gravitel) : base(gravitel)
        {
            
        }

        public async Task<IEnumerable<NumberDto>> GetNumbers()
        {
            var response = await _gravitel.SendCommand<List<NumberDto>>(HttpMethod.Get, "numbers");

            if (response is null)
                return Enumerable.Empty<NumberDto>();

            return response;
        }
    }
}
