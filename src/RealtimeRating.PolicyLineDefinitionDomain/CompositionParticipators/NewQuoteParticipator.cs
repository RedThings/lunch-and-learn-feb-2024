using RealtimeRating.Composition;
using RealtimeRating.Composition.NewQuote;
using RealtimeRating.PolicyLineDefinitionDomain.Grains;
using RealtimeRating.PolicyLineDefinitionDomain.Messages;

namespace RealtimeRating.PolicyLineDefinitionDomain.CompositionParticipators;

public class NewQuoteParticipator(IGrainFactory grainFactory) : IParticipateInViewModelComposition<NewQuoteRequest, NewQuoteResponse>
{
    public int ExecutionOrder => 0;

    public async Task Participate(NewQuoteRequest request, NewQuoteResponse response)
    {
        var service = grainFactory.GetGrain<IProvideAllPolicyLineDefinitions>(Guid.Empty);
        var policyLineDefinitions = await service.Ask(new GetAllPolicyLineDefinitions());

        response.PolicyLineDefinitions = policyLineDefinitions.Select(x => new PolicyLineDefinitionForNewQuote { Code = x.Code, Name = x.Name }).ToArray();
    }

    public Task Rollback(Exception exception) => Task.CompletedTask;
}