namespace RealtimeRating.PolicyLineDefinitionDomain.Dtos;

[Alias(nameof(PolicyLineDefinition))]
[GenerateSerializer]
public record PolicyLineDefinition
{
    [Id(0)]
    public required string Name { get; init; }

    [Id(1)]
    public required string Code { get; init; }
    
    [Id(2)]
    public required IReadOnlyCollection<Question> Questions { get; init; }
}