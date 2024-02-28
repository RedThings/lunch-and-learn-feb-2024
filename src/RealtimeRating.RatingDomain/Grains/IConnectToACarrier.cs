using RealtimeRating.RatingDomain.Messages;

namespace RealtimeRating.RatingDomain.Grains;

[Alias(nameof(IConnectToACarrier))]
public interface IConnectToACarrier : IGrainWithGuidKey
{
    [Alias(nameof(Tell) + nameof(FetchRatesFromCarrier))]
    Task Tell(FetchRatesFromCarrier message);
}