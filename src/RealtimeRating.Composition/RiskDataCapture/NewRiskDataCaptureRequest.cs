namespace RealtimeRating.Composition.RiskDataCapture;

public class NewRiskDataCaptureRequest : IRepresentARequestForComposition<NewRiskDataCaptureResponse>
{
    public required Guid QuoteId { get; init; }
    public required Guid RiskVariationId { get; init; }
    public required string PolicyLineDefinitionCode { get; init; }
}