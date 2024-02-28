using RealtimeRating.RatingDomain.Dtos;

namespace RealtimeRating.RatingDomain.Messages;

[Alias(nameof(CompleteRating))]
[GenerateSerializer]
public record CompleteRating
{
    [Id(0)]
    public required Rate[] Rates { get; init; }
}