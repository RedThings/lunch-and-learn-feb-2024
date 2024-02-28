namespace RedisChannels;

public interface IPublishToRedisChannels
{
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : class;
}