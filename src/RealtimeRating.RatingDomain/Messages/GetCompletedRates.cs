namespace RealtimeRating.RatingDomain.Messages;

[Alias(nameof(GetCompletedRates))]
[GenerateSerializer]
public record GetCompletedRates;