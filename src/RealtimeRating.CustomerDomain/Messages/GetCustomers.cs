namespace RealtimeRating.CustomerDomain.Messages;

[Alias(nameof(GetCustomers))]
[GenerateSerializer]
public record GetCustomers;