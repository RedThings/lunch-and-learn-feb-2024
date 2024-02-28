namespace RealtimeRating.RatingDomain.Messages;

[Alias(nameof(InitializeCompletedRatingSession))]
[GenerateSerializer]
public record InitializeCompletedRatingSession
{
    [Id(0)]
    public int NumberOfExpectedRates { get; set; }
}