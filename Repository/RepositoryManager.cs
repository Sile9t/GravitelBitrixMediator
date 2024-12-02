using OuterSource;
using Repository.Contracts;
using Repository.Contracts.Repos;
using Repository.Repos;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly Bitrix24Old _bitrix;
        private readonly Lazy<ITelephonyRepository> _telephony;
        private readonly Lazy<ICompanyRepository> _company;
        private readonly Lazy<ILeadRepository> _lead;
        private readonly Lazy<IDealRepository> _deal;
        private readonly Lazy<IContactRepository> _contact;

        public RepositoryManager(Bitrix24Old bitrix)
        {
            _bitrix = bitrix;
            _telephony = new Lazy<ITelephonyRepository>(() => new TelephonyRepository(_bitrix));
            _company = new Lazy<ICompanyRepository>(() => new CompanyRepository(_bitrix));
            _lead = new Lazy<ILeadRepository>(() => new LeadRepository(_bitrix));
            _deal = new Lazy<IDealRepository>(() => new DealRepository(_bitrix));
            _contact = new Lazy<IContactRepository>(() => new ContactRepository(_bitrix));
        }

        public ITelephonyRepository Telephony => _telephony.Value;
        public ICompanyRepository Company => _company.Value;
        public ILeadRepository Lead => _lead.Value;
        public IDealRepository Deal => _deal.Value;
        public IContactRepository Contact => _contact.Value;
    }
}
