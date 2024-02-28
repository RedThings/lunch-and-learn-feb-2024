namespace RealtimeRating.RatingDomain.Dtos;

[Alias(nameof(FetchingRatesResult))]
[GenerateSerializer]
public record FetchingRatesResult
{
    [Id(0)]
    public int NumberOfRatesExpected { get; set; }

    [Id(1)]
    public required bool Finished { get; init; }

    [Id(2)]
    public required IReadOnlyCollection<Rate> Rates { get; init; }
}