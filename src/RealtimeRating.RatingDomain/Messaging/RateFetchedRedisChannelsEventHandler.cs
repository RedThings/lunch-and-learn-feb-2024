using RealtimeRating.RatingDomain.Dtos;
using RealtimeRating.RatingDomain.Grains;
using RealtimeRating.RatingDomain.Messages;
using RedisChannels;

namespace RealtimeRating.RatingDomain.Messaging;

public class RateFetchedRedisChannelsEventHandler(IGrainFactory factory) : IHandleRedisChannelsEvents<RateFetched>
{
    public async Task HandleAsync(RateFetched message, CancellationToken cancellationToken)
    {
        var grain = factory.GetGrain<IRepresentARatingSession>(message.RatingSessionId);

        var rate = new Rate
        {
            Carrier = message.Rate.Carrier,
            Name = message.Rate.Name,
            Premium = message.Rate.Premium
        };

        await grain.Tell(new AddRate { Rate = rate }).ConfigureAwait(false);
    }
}