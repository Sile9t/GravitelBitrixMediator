using OuterSource;
using Repository.Contracts;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly Bitrix24Old _bitrix;
        private readonly Lazy<ITelephonyRepository> _telephony;
        private readonly Lazy<ICompanyRepository> _company;

        public RepositoryManager(Bitrix24Old bitrix)
        {
            _bitrix = bitrix;
            _telephony = new Lazy<ITelephonyRepository>(() => new TelephonyRepository(_bitrix));
            _company = new Lazy<ICompanyRepository>(() => new CompanyRepository(_bitrix));
        }

        public ITelephonyRepository Telephony => _telephony.Value;
        public ICompanyRepository Company => _company.Value;
    }
}
