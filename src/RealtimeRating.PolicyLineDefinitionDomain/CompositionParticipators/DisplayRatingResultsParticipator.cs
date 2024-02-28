using RealtimeRating.Composition;
using RealtimeRating.Composition.Rating;
using RealtimeRating.PolicyLineDefinitionDomain.Grains;
using RealtimeRating.PolicyLineDefinitionDomain.Messages;

namespace RealtimeRating.PolicyLineDefinitionDomain.CompositionParticipators;

public class DisplayRatingResultsParticipator(IGrainFactory factory) : IParticipateInViewModelComposition<DisplayRatingResultsRequest, DisplayRatingResultsResponse>
{
    public int ExecutionOrder => 0;

    public async Task Participate(DisplayRatingResultsRequest request, DisplayRatingResultsResponse response)
    {
        var grain = factory.GetGrain<IProvideAllPolicyLineDefinitions>(request.RiskVariationId);

        var all = await grain.Ask(new GetAllPolicyLineDefinitions());

        var pld = all.SingleOrDefault(x => x.Code == request.PolicyLineDefinitionCode);

        response.PolicyLineDefinitionName = pld == null ? "UNKNOWN PLD" : pld.Name;
    }

    public Task Rollback(Exception exception) => Task.CompletedTask;
}