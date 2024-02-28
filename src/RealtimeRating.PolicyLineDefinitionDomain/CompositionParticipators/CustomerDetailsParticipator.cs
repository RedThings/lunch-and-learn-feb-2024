using RealtimeRating.Composition;
using RealtimeRating.Composition.Customer;
using RealtimeRating.PolicyLineDefinitionDomain.Grains;
using RealtimeRating.PolicyLineDefinitionDomain.Messages;

namespace RealtimeRating.PolicyLineDefinitionDomain.CompositionParticipators;

public class CustomerDetailsParticipator(IGrainFactory grainFactory) : IParticipateInViewModelComposition<CustomerDetailsRequest, CustomerDetailsResponse>
{
    public int ExecutionOrder => 0;

    public async Task Participate(CustomerDetailsRequest request, CustomerDetailsResponse response)
    {
        if (request.Last5QuotesMetadata == null)
        {
            throw new Exception($"{nameof(request.Last5QuotesMetadata)} is null - execution order must be wrong");
        }

        var allPldsGrain = grainFactory.GetGrain<IProvideAllPolicyLineDefinitions>(Guid.Empty);
        var allPlds = await allPldsGrain.Ask(new GetAllPolicyLineDefinitions());

        response.Last5Quotes ??= request.Last5QuotesMetadata.Select(_ => new RatedQuoteSummary()).ToArray();

        for (var i = 0; i < response.Last5Quotes.Count; i++)
        {
            var code = request.Last5QuotesMetadata.ElementAt(i).PolicyLineDefinitionCode;
            var pldName = allPlds.Single(x => x.Code == code).Name;
            response.Last5Quotes.ElementAt(i).PolicyLineDefinitionName = pldName;
        }
    }

    public Task Rollback(Exception exception) => Task.CompletedTask;
}