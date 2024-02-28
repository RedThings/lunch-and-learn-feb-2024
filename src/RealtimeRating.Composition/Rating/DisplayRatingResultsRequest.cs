namespace RealtimeRating.Composition.Rating;

public class DisplayRatingResultsRequest : IRepresentARequestForComposition<DisplayRatingResultsResponse>
{
    public required Guid CustomerId { get; init; }
    public required Guid QuoteId { get; init; }
    public required Guid RiskVariationId { get; init; }
    public required string PolicyLineDefinitionCode { get; init; }
    public required Guid RatingSessionId { get; init; }
}