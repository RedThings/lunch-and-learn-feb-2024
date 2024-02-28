namespace RealtimeRating.PolicyLineDefinitionDomain.Dtos;

[Alias(nameof(Answer))]
[GenerateSerializer]
public record Answer
{
    [Id(0)]
    public required string Code { get; init; }

    [Id(1)]
    public required string? Value { get; init; }
}