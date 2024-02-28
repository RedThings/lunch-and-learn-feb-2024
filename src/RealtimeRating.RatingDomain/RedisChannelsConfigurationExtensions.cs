using RealtimeRating.RatingDomain.Messaging;
using RedisChannels;

namespace RealtimeRating.RatingDomain;

public static class RedisChannelsConfigurationExtensions
{
    public static RedisChannelsConfiguration RegisterRatingEventHandlers(this RedisChannelsConfiguration options)
    {
        options.RegisterEventHandler<CarrierIdentified, CarrierIdentifiedRedisChannelsEventHandler>();
        options.RegisterEventHandler<RateFetched, RateFetchedRedisChannelsEventHandler>();

        return options;
    }
}