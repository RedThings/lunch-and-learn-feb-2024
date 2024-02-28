using RealtimeRating.RatingDomain.Grains;
using RealtimeRating.RatingDomain.Messages;

namespace RealtimeRating.RatingDomain.Messaging;

public class CarrierIdentifiedNsbEventHandler(IGrainFactory factory) : IHandleMessages<CarrierIdentified>
{
    public async Task Handle(CarrierIdentified message, IMessageHandlerContext context)
    {
        var grain = factory.GetGrain<IConnectToACarrier>(message.CarrierConnectorId);

        await grain.Tell(new FetchRatesFromCarrier { RatingSessionId = message.RatingSessionId }).ConfigureAwait(false);
    }
}