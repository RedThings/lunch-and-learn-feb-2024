namespace RedisChannels;

public interface IHandleRedisChannelsEvents<in TEvent>
    where TEvent : class
{
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken);
}