using OuterSource;
using Repository.Contracts;

namespace Repository
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
