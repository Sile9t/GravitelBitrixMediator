using Repository.Contracts;
using Services.Bitrix;
using Services.Contracts;
using Services.Contracts.Bitrix;

namespace Services
{
    public class BitrixServiceManager : IBitrixServiceManager
    {
        private readonly Lazy<ITelephonyService> _telephony;
        private readonly Lazy<ICompanyService> _company;
        private readonly Lazy<ILeadService> _lead;
        private readonly Lazy<IDealService> _deal;
        private readonly Lazy<IContactService> _contact;
        private readonly Lazy<IGroupService> _group;

        public BitrixServiceManager(IBitrixRepositoryManager repo, RedisCacheService cache)
        {
            _telephony = new Lazy<ITelephonyService>(() => new TelephonyService(repo, cache));
            _company = new Lazy<ICompanyService>(() => new CompanyService(repo, cache));
            _lead = new Lazy<ILeadService>(() => new LeadService(repo, cache));
            _deal = new Lazy<IDealService>(() => new DealService(repo, cache));
            _contact = new Lazy<IContactService>(() => new ContactService(repo, cache));
            _group = new Lazy<IGroupService>(() => new GroupService(repo, cache));
        }

        public ITelephonyService Telephony => _telephony.Value;
        public ICompanyService Company => _company.Value;
        public ILeadService Lead => _lead.Value;
        public IDealService Deal => _deal.Value;
        public IContactService Contact => _contact.Value;
        public IGroupService Group => _group.Value;
    }
}
