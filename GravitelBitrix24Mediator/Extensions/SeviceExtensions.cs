using Contracts;
using LoggerService;
using OuterSource;
using Repository;
using Repository.Contracts;

namespace GravitelBitrix24Mediator.Extensions
{
    public static class SeviceExtensions
    {
        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddSingleton<ILoggerManager, LoggerManager>();
        
        public static void ConfigureOuterSources(this IServiceCollection services)
        {
            services.AddTransient<Bitrix24Old>();
            services.AddTransient<Gravitel>();
        }

        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddTransient<IBitrixRepositoryManager, BitrixRepositoryManager>();
    }
}
