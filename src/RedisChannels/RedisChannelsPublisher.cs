using System.Text.Json;
using StackExchange.Redis;

namespace RedisChannels;

public class RedisChannelsPublisher(IConnectionMultiplexer connection) : IPublishToRedisChannels
{
    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class
    {
        var subscriber = connection.GetSubscriber();
        var channel = RedisChannel.Literal(ChannelNameBuilder.BuildChannelName<TEvent>());
        var json = JsonSerializer.Serialize(@event);

        await subscriber.PublishAsync(channel, json, CommandFlags.FireAndForget).ConfigureAwait(false);
    }
}