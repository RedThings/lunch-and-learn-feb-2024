using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Exception = System.Exception;

namespace RedisChannels;

public class RedisChannelsBackgroundService(IServiceProvider serviceProvider) : BackgroundService
{
    private readonly Dictionary<string, ISubscriber> subscribers = [];

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<RedisChannelsBackgroundService>>();

        logger.LogInformation("Starting {name}", nameof(RedisChannelsBackgroundService));

        var configuration = serviceProvider.GetRequiredService<RedisChannelsConfiguration>();

        if (configuration.PublisherOnly)
        {
            logger.LogInformation("{name} is {property} - stopping", nameof(RedisChannelsBackgroundService), nameof(RedisChannelsConfiguration.PublisherOnly));
            
            await base.StopAsync(stoppingToken).ConfigureAwait(false);

            return;
        }

        IConnectionMultiplexer? connection = null;

        try
        {
            logger.LogInformation("Creating subscribers");

            connection = serviceProvider.GetRequiredService<IConnectionMultiplexer>();

            foreach (var eventHandlerWrapperKeyValue in configuration.EventHandlerWrappers)
            {
                var channelKey = eventHandlerWrapperKeyValue.Key;

                var subscriber = connection.GetSubscriber();

                subscribers[channelKey] = subscriber;

                var wrapper = eventHandlerWrapperKeyValue.Value;

                await subscriber
                    .SubscribeAsync(
                        RedisChannel.Literal(channelKey),
                        (_, value) => wrapper.OnNewMessage(value, serviceProvider, stoppingToken),
                        CommandFlags.FireAndForget
                    ).ConfigureAwait(false);
            }

            logger.LogInformation("Complete - service will continue until cancellation...");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(100, stoppingToken).ConfigureAwait(false);
            }

            foreach (var _ in subscribers)
            {
                // do nothing // not sure what to do with 'em //
            }
        }
        catch (Exception ex)
        {
            logger.LogError("An exception occurred in the {name}: {ex} {innerEx}", nameof(RedisChannelsBackgroundService), ex.Message, ex.InnerException?.Message);
        }
        finally
        {
            if (connection != null)
            {
                await connection.DisposeAsync().ConfigureAwait(false);
            }
        }
    }
}