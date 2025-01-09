using Contracts;
using Repository.Contracts;
using Services.Contracts;

namespace Services
{
    public class ServiceManager
    {
        private Lazy<ICallService> _callService;
        private Lazy<IEventService> _eventService;

        public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger)
        {
            _callService = new Lazy<ICallService>(() => 
                new CallService(repositoryManager, logger));
            _eventService = new Lazy<IEventService>(() =>
                new EventService(repositoryManager, logger));
        }

        public ICallService CallService => _callService.Value;
        public IEventService EventService => _eventService.Value;
    }
}
