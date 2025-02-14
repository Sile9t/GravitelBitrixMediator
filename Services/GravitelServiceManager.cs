using Repository.Contracts;
using Services.Contracts;
using Services.Contracts.Gravitel;
using Services.Gravitel;

namespace Services
{
    public class GravitelServiceManager : IGravitelServiceManager
    {
        private readonly Lazy<IAccountService> _account;
        private readonly Lazy<IGroupService> _group;
        private readonly Lazy<INumberService> _number;

        public GravitelServiceManager(IGravitelRepositoryManager repo, RedisCacheService cache)
        {
            _account = new Lazy<IAccountService>(() => new AccountService(repo, cache));  
            _group = new Lazy<IGroupService>(() => new GroupService(repo, cache));  
            _number = new Lazy<INumberService>(() => new NumberService(repo, cache));  
        }

        public IAccountService Account => _account.Value;

        public IGroupService Group => _group.Value;

        public INumberService Number => _number.Value;
    }
}
