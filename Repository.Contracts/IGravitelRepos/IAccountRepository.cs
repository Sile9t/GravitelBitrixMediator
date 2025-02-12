using Entities.Dtos.Gravitel;

namespace Repository.Contracts.IGravitelRepos
{
    public interface IAccountRepository
    {
        Task<List<AccountDto>?> GetAccounts();
    }
}
