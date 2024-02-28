namespace RealtimeRating.PolicyLineDefinitionDomain.Dtos;

[Alias(nameof(DropDownOption))]
[GenerateSerializer]
public record DropDownOption
{
    [Id(0)]
    public required string Name { get; init; }
    
    [Id(1)]
    public required string Value { get; init; }
}