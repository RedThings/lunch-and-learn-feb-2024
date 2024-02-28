using RealtimeRating.Composition;
using RealtimeRating.Composition.Rating;
using RealtimeRating.QuoteDomain.Grains;
using RealtimeRating.QuoteDomain.Messages;

namespace RealtimeRating.QuoteDomain.CompositionParticipators;

public class DisplayRatingResultsParticipator(IGrainFactory factory) : IParticipateInViewModelComposition<DisplayRatingResultsRequest, DisplayRatingResultsResponse>
{
    public int ExecutionOrder => 0;

    public async Task Participate(DisplayRatingResultsRequest request, DisplayRatingResultsResponse response)
    {
        var grain = factory.GetGrain<IRepresentAQuote>(request.QuoteId);

        var details = await grain.Ask(new GetQuoteRiskVariationDetails { Id = request.RiskVariationId });

        response.RiskVariationName = details.Name;
    }

    public Task Rollback(Exception exception) => Task.CompletedTask;
}