namespace RealtimeRating.Composition.RiskDataCapture;

public class QuestionForRiskDataCapture
{
    public required string Code { get; init; }
    public required string Label { get; init; }
    public IReadOnlyCollection<DropDownOptionForRiskDataCapture>? Options { get; init; }
    public required QuestionTypeForRiskDataCapture Type { get; init; }
    public string? Answer { get; set; }
    public string? ValidationError { get; set; }
}