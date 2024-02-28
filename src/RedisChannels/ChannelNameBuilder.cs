namespace RedisChannels;

public static class ChannelNameBuilder
{
    public static string BuildChannelName<TEvent>()
        where TEvent : class
        => "topic-name-" + typeof(TEvent).Name;
}