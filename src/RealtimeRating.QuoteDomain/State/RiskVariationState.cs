namespace RealtimeRating.QuoteDomain.State;

[Alias(nameof(RiskVariationState))]
[GenerateSerializer]
public class RiskVariationState
{
    [Id(0)]
    public Guid Id { get; set; }

    [Id(1)]
    public string Name { get; set; } = string.Empty;
}