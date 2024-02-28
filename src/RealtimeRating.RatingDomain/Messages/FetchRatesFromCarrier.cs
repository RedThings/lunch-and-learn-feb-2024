namespace RealtimeRating.RatingDomain.Messages;

[Alias(nameof(FetchRatesFromCarrier))]
[GenerateSerializer]
public record FetchRatesFromCarrier
{
    [Id(0)]
    public required Guid RatingSessionId { get; init; }
}