using RealtimeRating.RatingDomain.Dtos;
using RealtimeRating.RatingDomain.Grains;
using RealtimeRating.RatingDomain.Messages;

namespace RealtimeRating.RatingDomain.Messaging;

public class RateFetchedNsbEventHandler(IGrainFactory factory) : IHandleMessages<RateFetched>
{
    public async Task Handle(RateFetched message, IMessageHandlerContext context)
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