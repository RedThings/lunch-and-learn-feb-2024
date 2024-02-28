namespace RealtimeRating.CustomerQuoteDomain.Messages;

[Alias(nameof(AddCustomerQuoteMetadata))]
[GenerateSerializer]
public record AddCustomerQuoteMetadata
{
    [Id(0)]
    public string PolicyLineDefinitionCode { get; set; } = string.Empty;

    [Id(1)]
    public Guid QuoteId { get; set; }

    [Id(2)]
    public Guid RiskVariationId { get; set; }

    [Id(3)]
    public Guid RatingSessionId { get; set; }
}