using RealtimeRating.RatingDomain.Dtos;
using RealtimeRating.RatingDomain.Messages;

namespace RealtimeRating.RatingDomain.Grains;

[Alias(nameof(IRepresentACompletedRatingSession))]
public interface IRepresentACompletedRatingSession : IGrainWithGuidKey
{
    [Alias(nameof(Tell) + nameof(InitializeCompletedRatingSession))]
    Task Tell(InitializeCompletedRatingSession message);

    [Alias(nameof(Tell) + nameof(CompleteRating))]
    Task Tell(CompleteRating message);

    [Alias(nameof(Ask) + nameof(IsRatingCompleted))]
    Task<bool> Ask(IsRatingCompleted message);

    [Alias(nameof(Ask) + nameof(GetNumberOfExpectedRates))]
    Task<int> Ask(GetNumberOfExpectedRates message);

    [Alias(nameof(Ask) + nameof(GetCompletedRates))]
    Task<IReadOnlyCollection<Rate>> Ask(GetCompletedRates message);
}