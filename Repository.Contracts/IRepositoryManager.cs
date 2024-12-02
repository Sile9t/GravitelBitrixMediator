namespace Repository.Contracts
{
    public interface IRepositoryManager
    {
        ITelephonyRepository Telephony { get; }
        ICompanyRepository Company { get; }
    }
}
