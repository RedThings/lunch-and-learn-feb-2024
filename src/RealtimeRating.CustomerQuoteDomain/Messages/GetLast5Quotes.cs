namespace RealtimeRating.CustomerQuoteDomain.Messages;

[Alias(nameof(GetLast5Quotes))]
[GenerateSerializer]
public record GetLast5Quotes;