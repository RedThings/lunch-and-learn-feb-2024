using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace RedisChannels;

public static class HostBuilderExtensions
{
    public static IHostBuilder UseRedisChannels(this IHostBuilder host, Action<HostBuilderContext, RedisChannelsConfiguration> configurationAction)
    {
        host.ConfigureServices((hostBuilderContext, services) =>
        {
            var configuration = new RedisChannelsConfiguration();

            configurationAction.Invoke(hostBuilderContext, configuration);

            var connection = ConnectionMultiplexer.Connect(configuration.ConnectionString);

            services.AddSingleton(configuration);
            services.AddSingleton<IConnectionMultiplexer>(connection);
            services.AddScoped<IPublishToRedisChannels, RedisChannelsPublisher>();
            services.AddHostedService<RedisChannelsBackgroundService>();

            foreach (var wrapper in configuration.EventHandlerWrappers.Select(x => x.Value))
            {
                services.AddScoped(wrapper.EventHandlerType);
            }
        });

        return host;
    }
}
