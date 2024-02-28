namespace RealtimeRating.Composition.RiskDataCapture;

public class SubmitNewRiskDataCaptureRequest : IRepresentARequestForComposition<SubmitNewRiskDataCaptureResponse>
{
    public required Guid QuoteId { get; set; }
    public required Guid RiskVariationId { get; set; }
    public required string PolicyLineDefinitionCode { get; set; }
    public required IReadOnlyCollection<KeyValuePair<string, string?>> Answers { get; init; }
}