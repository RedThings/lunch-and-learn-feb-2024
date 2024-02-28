namespace RealtimeRating.CustomerQuoteDomain.Dtos;

[Alias(nameof(CustomerQuoteMetadata))]
[GenerateSerializer]
public record CustomerQuoteMetadata
{
    [Id(0)]
    public required Guid QuoteId { get; set; }

    [Id(1)] 
    public required string PolicyLineDefinitionCode { get; set; }

    [Id(2)]
    public required Guid RiskVariationId { get; set; }

    [Id(3)]
    public required Guid RatingSessionId { get; set; }

    [Id(4)]
    public required DateTime Added { get; set; }
}