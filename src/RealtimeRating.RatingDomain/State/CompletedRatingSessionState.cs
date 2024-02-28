using RealtimeRating.RatingDomain.Dtos;

namespace RealtimeRating.RatingDomain.State;

[Alias(nameof(CompletedRatingSessionState))]
[GenerateSerializer]
public record CompletedRatingSessionState
{
    [Id(0)]
    public int NumberOfExpectedRates { get; set; }

    [Id(1)]
    public bool Completed { get; set; }

    [Id(2)]
    public IReadOnlyCollection<Rate> Rates { get; set; } = [];
}