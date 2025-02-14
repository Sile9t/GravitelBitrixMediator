using Contracts;
using Microsoft.Extensions.Caching.Distributed;
using Repository.Contracts;
using Services.Contracts;

namespace Services
{
    public class ServiceManager
    {
        private Lazy<IDistributedCache> _cache;
        private Lazy<ICallService> _callService;
        private Lazy<IEventService> _eventService;

        public ServiceManager(IBitrixRepositoryManager bitrix, IGravitelRepositoryManager gravitel, ILoggerManager logger)
        {
            _callService = new Lazy<ICallService>(() => 
                new CallService(bitrix, gravitel, logger));
            _eventService = new Lazy<IEventService>(() =>
                new EventService(bitrix, gravitel, logger));
        }

        public ICallService CallService => _callService.Value;
        public IEventService EventService => _eventService.Value;
    }
}
