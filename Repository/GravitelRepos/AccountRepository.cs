using Entities.Dtos.Gravitel;
using OuterSource;
using Repository.Contracts.IGravitelRepos;

namespace Repository.GravitelRepos
{
    public class AccountRepository : GravitelRepositoryBase, IAccountRepository
    {
        public AccountRepository(Gravitel gravitel) : base(gravitel)
        {
            
        }
        public async Task<List<AccountDto>?> GetAccounts()
        {
            var response = await _gravitel.SendCommand<List<AccountDto>>(HttpMethod.Get, "accounts");

            return response;
        }
    }
}
