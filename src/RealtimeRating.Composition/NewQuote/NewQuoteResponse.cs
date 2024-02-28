namespace RealtimeRating.Composition.NewQuote;

public class NewQuoteResponse : IRepresentAComposedResponse
{
    public PolicyLineDefinitionForNewQuote[] PolicyLineDefinitions { get; set; } = [];
}