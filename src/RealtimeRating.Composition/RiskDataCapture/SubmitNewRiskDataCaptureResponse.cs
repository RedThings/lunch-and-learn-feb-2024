namespace RealtimeRating.Composition.RiskDataCapture;

public class SubmitNewRiskDataCaptureResponse : IRepresentAComposedResponse
{
    public bool Valid => Questions.All(x => x.ValidationError == null);
    public required IReadOnlyCollection<QuestionForRiskDataCapture> Questions { get; set; }
}