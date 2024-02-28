namespace RealtimeRating.CustomerDomain.Dtos;

[Alias(nameof(Customer))]
[GenerateSerializer]
public record Customer
{
    [Id(0)]
    public Guid Id { get; set; }

    [Id(1)]
    public string LookupCode { get; set; } = string.Empty;

    [Id(2)]
    public string Name { get; set; } = string.Empty;
}