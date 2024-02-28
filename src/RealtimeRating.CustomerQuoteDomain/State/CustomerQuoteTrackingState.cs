using RealtimeRating.CustomerQuoteDomain.Dtos;

namespace RealtimeRating.CustomerQuoteDomain.State;

[Alias(nameof(CustomerQuoteTrackingState))]
[GenerateSerializer]
public class CustomerQuoteTrackingState
{
    [Id(0)]
    public List<CustomerQuoteMetadata> CustomerQuoteMetadatas { get; set; } = [];
}