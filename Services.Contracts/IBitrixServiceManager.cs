using Services.Contracts.Bitrix;

namespace Services.Contracts
{
    public interface IBitrixServiceManager
    {
        ITelephonyService Telephony { get; }
        ICompanyService Company { get; }
        ILeadService Lead { get; }
        IDealService Deal { get; }
        IContactService Contact { get; }
        IGroupService Group { get; }
    }
}
