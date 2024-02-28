namespace RealtimeRating.Composition.Customer;

public class QuoteMetadataForCustomerDetailsRequest
{
    public required Guid QuoteId { get; init; }
    public required string PolicyLineDefinitionCode { get; init; }
    public required Guid RiskVariationId { get; init; }
    public required Guid RatingSessionId { get; init; }
    public required DateTime Added { get; init; }
}