namespace RealtimeRating.Composition.Customer;

public class CustomerDetailsResponse : IRepresentAComposedResponse
{
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerLookupCode { get; set; } = string.Empty;
    public IReadOnlyCollection<RatedQuoteSummary>? Last5Quotes { get; set; }
}