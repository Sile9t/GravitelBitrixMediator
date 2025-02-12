using Repository.Contracts.Repos;

namespace Repository.Contracts
{
    public interface IBitrixRepositoryManager
    {
        ITelephonyRepository Telephony { get; }
        ICompanyRepository Company { get; }
        ILeadRepository Lead { get; }
        IDealRepository Deal { get; }
        IContactRepository Contact { get; }
        IGroupRepository Group { get; }
    }
}
