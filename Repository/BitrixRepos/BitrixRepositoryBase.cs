using OuterSource;
using Repository.Contracts.Repos;

namespace Repository.Repos
{
    public class BitrixRepositoryBase : IRepositoryBase
    {
        protected Bitrix24Old _bitrix;

        public BitrixRepositoryBase(Bitrix24Old bitrix)
        {
            _bitrix = bitrix;
        }
    }
}
