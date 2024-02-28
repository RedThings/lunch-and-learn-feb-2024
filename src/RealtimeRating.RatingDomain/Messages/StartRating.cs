namespace RealtimeRating.RatingDomain.Messages;

[Alias(nameof(StartRating))]
[GenerateSerializer]
public record StartRating
{
    [Id(0)]
    public required string PolicyLineDefinitionCode { get; init; }

    [Id(1)]
    public required string RiskData { get; init; }
};