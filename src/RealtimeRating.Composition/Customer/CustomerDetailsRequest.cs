namespace RealtimeRating.Composition.Customer;

public class CustomerDetailsRequest : IRepresentARequestForComposition<CustomerDetailsResponse>
{
    public required Guid CustomerId { get; set; }
    public IReadOnlyCollection<QuoteMetadataForCustomerDetailsRequest>? Last5QuotesMetadata { get; set; }
}