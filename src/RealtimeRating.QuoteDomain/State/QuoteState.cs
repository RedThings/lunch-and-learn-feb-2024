namespace RealtimeRating.QuoteDomain.State;

[Alias(nameof(QuoteState))]
[GenerateSerializer]
public class QuoteState
{
    [Id(0)]
    public List<RiskVariationState> RiskVariations { get; set; } = [];
}