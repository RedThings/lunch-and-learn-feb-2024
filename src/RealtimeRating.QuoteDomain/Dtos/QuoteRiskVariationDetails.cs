namespace RealtimeRating.QuoteDomain.Dtos;

[Alias(nameof(QuoteRiskVariationDetails))]
[GenerateSerializer]
public record QuoteRiskVariationDetails
{
    [Id(0)]
    public required string Name { get; init; }
}