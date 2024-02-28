using RealtimeRating.PolicyLineDefinitionDomain.Dtos;

namespace RealtimeRating.PolicyLineDefinitionDomain.State;

[Alias(nameof(PolicyLineDefinitionsState))]
[GenerateSerializer]
public class PolicyLineDefinitionsState
{
    [Id(0)]
    public IReadOnlyCollection<PolicyLineDefinition>? Definitions { get; set; }
}