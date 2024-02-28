using RealtimeRating.RatingDomain.Messages;

namespace RealtimeRating.RatingDomain.Grains;

[Alias(nameof(IRepresentARatingSession))]
public interface IRepresentARatingSession : IGrainWithGuidKey
{
    [Alias(nameof(Tell) + nameof(StartRating))]
    Task Tell(StartRating message);

    [Alias(nameof(Tell) + nameof(DeactivateRatingSession))]
    Task Tell(DeactivateRatingSession message);
}