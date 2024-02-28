namespace RealtimeRating.QuoteDomain.Messages;

[Alias(nameof(GetQuoteRiskVariationDetails))]
[GenerateSerializer]
public record GetQuoteRiskVariationDetails
{
    [Id(0)]
    public required Guid Id { get; init; }
}