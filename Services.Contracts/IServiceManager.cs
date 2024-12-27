namespace Services.Contracts
{
    public interface IServiceManager
    {
        ICallService CallService { get; }
        IEventSevice EventService { get; }
    }
}
