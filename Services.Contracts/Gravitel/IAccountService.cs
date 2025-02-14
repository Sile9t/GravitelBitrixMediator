using Entities.Dtos.Gravitel;

namespace Services.Contracts.Gravitel
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountDto>> GetAccounts();
    }
}
