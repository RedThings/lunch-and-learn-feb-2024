namespace RealtimeRating.CustomerQuoteDomain.Messages;

[Alias(nameof(RemoveCustomerQuoteMetadata))]
[GenerateSerializer]
public record RemoveCustomerQuoteMetadata
{
    [Id(0)]
    public Guid RiskVariationId { get; set; }
}