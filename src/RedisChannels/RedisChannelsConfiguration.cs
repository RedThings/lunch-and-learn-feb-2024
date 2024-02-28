namespace RedisChannels;

public class RedisChannelsConfiguration
{
    private readonly Dictionary<string, EventHandlerWrapper> wrappers = [];

    public string ConnectionString { get; set; } = string.Empty;
    public bool PublisherOnly { get; set; }

    public IReadOnlyDictionary<string, EventHandlerWrapper> EventHandlerWrappers => wrappers;
    
    public void RegisterEventHandler<TEvent, TEventHandler>()
        where TEvent : class
        where TEventHandler : IHandleRedisChannelsEvents<TEvent>
    {
        var key = ChannelNameBuilder.BuildChannelName<TEvent>();

        wrappers[key] = EventHandlerWrapper.Create<TEvent, TEventHandler>();
    }
}