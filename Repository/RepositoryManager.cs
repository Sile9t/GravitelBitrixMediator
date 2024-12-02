using OuterSource;
using Repository.Contracts;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly Bitrix24Old _bitrix;
        private readonly Lazy<ITelephonyRepository> _telephony;

        public RepositoryManager(Bitrix24Old bitrix)
        {
            _bitrix = bitrix;
            _telephony = new Lazy<ITelephonyRepository>(() => new TelephonyRepository(_bitrix));
        }
        public ITelephonyRepository Telephony => _telephony.Value;
    }
}
