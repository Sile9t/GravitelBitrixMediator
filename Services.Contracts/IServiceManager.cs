namespace Services.Contracts
{
    public interface IServiceManager
    {
        ICallService CallService { get; }
        IEventService EventService { get; }
    }
}
