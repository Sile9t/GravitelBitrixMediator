using Contracts;
using Repository.Contracts;
using Services.Contracts;

namespace Services
{
    public class ServiceManager
    {
        private Lazy<ICallService> _callService;

        public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger)
        {
            _callService = new Lazy<ICallService>(() => 
                new CallService(repositoryManager, logger));
        }

        public ICallService CallService => _callService.Value;
    }
}
