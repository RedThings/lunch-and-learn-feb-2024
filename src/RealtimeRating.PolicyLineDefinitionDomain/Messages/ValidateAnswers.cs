using RealtimeRating.PolicyLineDefinitionDomain.Dtos;

namespace RealtimeRating.PolicyLineDefinitionDomain.Messages;

[Alias(nameof(ValidateAnswers))]
[GenerateSerializer]
public record ValidateAnswers
{
    [Id(0)]
    public required string PolicyLineDefinitionCode { get; init; }

    [Id(1)]
    public required IReadOnlyCollection<Answer> Answers { get; init; }
}