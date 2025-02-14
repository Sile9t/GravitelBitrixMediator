using Services.Contracts.Gravitel;

namespace Services.Contracts
{
    public interface IGravitelServiceManager
    {
        IAccountService Account {  get; }
        IGroupService Group {  get; }
        INumberService Number {  get; }
    }
}
