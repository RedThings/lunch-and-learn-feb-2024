using RealtimeRating.Composition.Rating;
using RealtimeRating.Composition;
using RealtimeRating.RatingDomain.Grains;
using RealtimeRating.RatingDomain.Messages;

namespace RealtimeRating.RatingDomain.CompositionParticipators;

public class DisplayRatingResultsParticipator(IGrainFactory factory) : IParticipateInViewModelComposition<DisplayRatingResultsRequest, DisplayRatingResultsResponse>
{
    public int ExecutionOrder => 0;

    public async Task Participate(DisplayRatingResultsRequest request, DisplayRatingResultsResponse response)
    {
        var grain = factory.GetGrain<IRepresentARatingSession>(request.RatingSessionId);

        var results = await grain.Ask(new GetFetchingRatesResult());

        response.NumberOfRatesExpected = results.NumberOfRatesExpected;
        response.FinishedRating = results.Finished;
        response.Rates = results.Rates.OrderBy(x => x.Premium).Select(x => new RateResult
        {
            Carrier = x.Carrier,
            Name = x.Name,
            Premium = x.Premium.ToString("F")
        }).ToArray();
    }

    public Task Rollback(Exception exception) => Task.CompletedTask;
}