using RealtimeRating.RatingDomain.Grains;
using RealtimeRating.RatingDomain.Messages;
using RedisChannels;

namespace RealtimeRating.RatingDomain.Messaging;

public class CarrierIdentifiedRedisChannelsEventHandler(IGrainFactory factory) : IHandleRedisChannelsEvents<CarrierIdentified>
{
    public async Task HandleAsync(CarrierIdentified message, CancellationToken cancellationToken)
    {
        var grain = factory.GetGrain<IConnectToACarrier>(message.CarrierConnectorId);

        await grain.Tell(new FetchRatesFromCarrier { RatingSessionId = message.RatingSessionId }).ConfigureAwait(false);
    }
}