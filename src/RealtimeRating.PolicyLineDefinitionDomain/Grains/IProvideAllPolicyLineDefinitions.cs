using RealtimeRating.PolicyLineDefinitionDomain.Dtos;
using RealtimeRating.PolicyLineDefinitionDomain.Messages;

namespace RealtimeRating.PolicyLineDefinitionDomain.Grains;

[Alias(nameof(IProvideAllPolicyLineDefinitions))]
public interface IProvideAllPolicyLineDefinitions : IGrainWithGuidKey
{
    [Alias(nameof(Ask) + nameof(GetAllPolicyLineDefinitions))]
    Task<IReadOnlyCollection<PolicyLineDefinition>> Ask(GetAllPolicyLineDefinitions message);

    [Alias(nameof(Ask) + nameof(ValidateAnswers))]
    Task<IReadOnlyCollection<ValidatedQuestion>> Ask(ValidateAnswers message);
}