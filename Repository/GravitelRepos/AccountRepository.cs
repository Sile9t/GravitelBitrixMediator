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
        public async Task<IEnumerable<AccountDto>> GetAccounts()
        {
            var response = await _gravitel.SendCommand<IEnumerable<AccountDto>>(HttpMethod.Get, "accounts");

            if (response is null)
                return Enumerable.Empty<AccountDto>();

            return response;
        }
    }
}
