namespace RealtimeRating.PolicyLineDefinitionDomain.Dtos;

[Alias(nameof(ValidatedQuestion))]
[GenerateSerializer]
public record ValidatedQuestion : Question
{
    [Id(0)]
    public required string? Answer { get; init; }

    [Id(1)]
    public required string? ValidationError { get; init; }
}