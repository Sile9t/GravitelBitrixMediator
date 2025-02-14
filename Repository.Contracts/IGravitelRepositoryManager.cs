using Repository.Contracts.IGravitelRepos;

namespace Repository.Contracts
{
    public interface IGravitelRepositoryManager
    {
        INumberRepository Number { get; }
        IGroupRepository Group { get; }
        IAccountRepository Account { get; }
    }
}
