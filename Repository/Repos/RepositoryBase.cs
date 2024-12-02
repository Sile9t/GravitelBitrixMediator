using OuterSource;
using Repository.Contracts.Repos;

namespace Repository.Repos
{
    public class RepositoryBase : IRepositoryBase
    {
        protected Bitrix24Old _bitrix;

        public RepositoryBase(Bitrix24Old bitrix)
        {
            _bitrix = bitrix;
        }
    }
}
