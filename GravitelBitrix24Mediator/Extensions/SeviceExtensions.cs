using Contracts;

namespace GravitelBitrix24Mediator.Extensions
{
    public static class SeviceExtensions
    {
        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddSingleton<ILoggerManager, ILoggerManager>();
    }
}
