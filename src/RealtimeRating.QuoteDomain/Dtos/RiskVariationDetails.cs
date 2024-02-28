namespace RealtimeRating.QuoteDomain.Dtos;

[Alias(nameof(RiskVariationDetails))]
[GenerateSerializer]
public class RiskVariationDetails
{
    [Id(0)]
    public string Name { get; set; } = string.Empty;
}