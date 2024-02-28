namespace RealtimeRating.Composition.NewQuote;

public record SubmitNewQuoteRequest : IRepresentARequestForComposition<SubmitNewQuoteResponse>
{
   public required string Name { get; init; }
}