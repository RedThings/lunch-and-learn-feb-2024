namespace RealtimeRating.Composition.Rating;

public class DisplayRatingResultsResponse : IRepresentAComposedResponse
{
    public IReadOnlyCollection<RateResult>? Rates { get; set; }
    public int NumberOfRatesExpected { get; set; }
    public bool FinishedRating { get; set; }
    public string RiskVariationName { get; set; } = string.Empty;
    public string PolicyLineDefinitionName { get; set; } = string.Empty;
}