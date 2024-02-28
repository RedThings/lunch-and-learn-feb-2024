namespace RealtimeRating.QuoteDomain.Messages;

[Alias(nameof(AddRiskVariation))]
[GenerateSerializer]
public record AddRiskVariation
{
    [Id(0)]
    public required string Name { get; init; }
    
    [Id(1)]
    public required Guid Id { get; init; }
}