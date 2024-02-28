namespace RealtimeRating.RatingDomain.State;

[Alias(nameof(RatingSessionState))]
[GenerateSerializer]
public class RatingSessionState
{
    [Id(0)]
    public int NumberOfRatesExpected { get; set; }
}