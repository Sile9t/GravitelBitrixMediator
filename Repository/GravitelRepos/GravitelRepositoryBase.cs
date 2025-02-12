using OuterSource;
using Repository.Contracts.Repos;

namespace Repository.GravitelRepos
{
    public class GravitelRepositoryBase : IRepositoryBase
    {
        protected Gravitel _gravitel;

        public GravitelRepositoryBase(Gravitel gravitel)
        {
            _gravitel = gravitel;
        }
    }
}
