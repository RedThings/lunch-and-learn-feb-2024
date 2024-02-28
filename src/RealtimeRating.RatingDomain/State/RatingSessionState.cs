using RealtimeRating.RatingDomain.Dtos;

namespace RealtimeRating.RatingDomain.State;

[Alias(nameof(RatingSessionState))]
[GenerateSerializer]
public class RatingSessionState
{
    [Id(0)]
    public DateTime? FinishedAt { get; set; }

    [Id(1)]
    public int NumberOfRatesExpected { get; set; }

    [Id(2)]
    public List<Rate>? Rates { get; set; }
}