namespace RealtimeRating.Composition.Rating;

public class StartRatingRequest : IRepresentARequestForComposition<StartRatingResponse>
{
    public required string PolicyLineDefinitionCode { get; init; }
    public required string RiskData { get; init; }
    public required Guid CustomerId { get; init; }
    public required Guid QuoteId { get; init; }
    public required Guid RiskVariationId { get; init; }
    public required bool DoNotRate { get; init; } // for tests
}