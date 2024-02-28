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
        var completedGrain = factory.GetGrain<IRepresentACompletedRatingSession>(request.RatingSessionId);

        var completed = await completedGrain.Ask(new IsRatingCompleted());

        if (!completed)
        {
            response.Rates = [];
            return;
        }

        var numberOfExpectedRates = await completedGrain.Ask(new GetNumberOfExpectedRates());
        var rates = await completedGrain.Ask(new GetCompletedRates());

        response.NumberOfRatesExpected = numberOfExpectedRates;
        response.FinishedRating = true;
        response.Rates = rates.OrderBy(x => x.Premium).Select(x => new RateResult
        {
            Carrier = x.Carrier,
            Name = x.Name,
            Premium = x.Premium.ToString("F")
        }).ToArray();
    }

    public Task Rollback(Exception exception) => Task.CompletedTask;
}