using Contracts;
using GravitelBitrix24Mediator.Extensions;
using NLog;
using OuterSource;
using Services;
using StackExchange.Redis;

namespace GravitelBitrix24Mediator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            LogManager.Setup().LoadConfigurationFromFile(
                string.Concat(Directory.GetCurrentDirectory(), @"/nlog.config"));

            builder.Services.ConfigureLoggerService();
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("RedisConn");
                options.InstanceName = "Cache";
            });
            builder.Services.AddTransient<RedisCacheService>();
            builder.Services.ConfigureOuterSources();
            //builder.Services.AddTransient<Bitrix24Old>();
            //builder.Services.AddTransient<Gravitel>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            var logger = app.Services.GetRequiredService<ILoggerManager>();
            app.ConfigureExceptionHandler(logger);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
