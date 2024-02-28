namespace RealtimeRating.Composition.RiskDataCapture;

public class NewRiskDataCaptureResponse : IRepresentAComposedResponse
{
    public string PolicyLineDefinitionName { get; set; } = string.Empty;
    public string RiskVariationName { get; set; } = string.Empty;
    public IReadOnlyCollection<QuestionForRiskDataCapture> Questions { get; set; } = [];
}