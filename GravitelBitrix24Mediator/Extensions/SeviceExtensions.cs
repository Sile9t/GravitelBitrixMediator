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

        public static void ConfigureOuterSource(this IServiceCollection services) =>
            services.AddScoped<Bitrix24Old>();

        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped<IBitrixRepositoryManager, BitrixRepositoryManager>();
    }
}
