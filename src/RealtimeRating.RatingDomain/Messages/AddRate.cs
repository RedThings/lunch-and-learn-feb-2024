using RealtimeRating.RatingDomain.Dtos;

namespace RealtimeRating.RatingDomain.Messages;

[Alias(nameof(AddRate))]
[GenerateSerializer]
public record AddRate
{
    [Id(0)]
    public required Rate Rate { get; init; }
}