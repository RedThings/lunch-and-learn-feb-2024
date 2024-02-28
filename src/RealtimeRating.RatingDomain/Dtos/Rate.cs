namespace RealtimeRating.RatingDomain.Dtos;

[Alias(nameof(Rate))]
[GenerateSerializer]
public record Rate
{
    [Id(0)]
    public required string Carrier { get; init; }

    [Id(1)]
    public required string Name { get; init; }

    [Id(2)]
    public required decimal Premium { get; init; }
}