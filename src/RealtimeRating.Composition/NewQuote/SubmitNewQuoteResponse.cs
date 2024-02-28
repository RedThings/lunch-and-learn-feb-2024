namespace RealtimeRating.Composition.NewQuote;

public class SubmitNewQuoteResponse : IRepresentAComposedResponse
{
    public Guid QuoteId { get; set; }
    public Guid RiskVariationId { get; set; }
}