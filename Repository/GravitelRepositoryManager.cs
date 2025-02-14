using OuterSource;
using Repository.Contracts;
using Repository.Contracts.IGravitelRepos;
using Repository.GravitelRepos;

namespace Repository
{
    public class GravitelRepositoryManager : IGravitelRepositoryManager
    {
        private readonly Gravitel _gravitel;
        private readonly Lazy<INumberRepository> _number;
        private readonly Lazy<IGroupRepository> _group;
        private readonly Lazy<IAccountRepository> _account;

        public GravitelRepositoryManager(Gravitel gravitel)
        {
            _gravitel = gravitel;
            _number = new Lazy<INumberRepository>(() => new NumberRepository(_gravitel));
            _group = new Lazy<IGroupRepository>(() => new GroupRepository(_gravitel));
            _account = new Lazy<IAccountRepository>(() => new AccountRepository(_gravitel));
        }

        public INumberRepository Number => _number.Value;
        public IGroupRepository Group => _group.Value;
        public IAccountRepository Account => _account.Value;
    }
}
