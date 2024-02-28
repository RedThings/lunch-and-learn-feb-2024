namespace RealtimeRating.PolicyLineDefinitionDomain.Dtos;

[Alias(nameof(Question))]
[GenerateSerializer]
public record Question
{
    [Id(0)]
    public required string Code { get; init; }

    [Id(1)]
    public required string Label { get; init; }
    
    [Id(2)]
    public required QuestionType Type { get; init; }
    
    [Id(3)]
    public IReadOnlyCollection<DropDownOption>? Options { get; init; }
}