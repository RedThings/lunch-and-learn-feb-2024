using RealtimeRating.Composition;
using RealtimeRating.Composition.Customer;
using RealtimeRating.RatingDomain.Grains;
using RealtimeRating.RatingDomain.Messages;

namespace RealtimeRating.RatingDomain.CompositionParticipators;

public class CustomerDetailsParticipator(IGrainFactory grainFactory) : IParticipateInViewModelComposition<CustomerDetailsRequest, CustomerDetailsResponse>
{
    public int ExecutionOrder => 0;

    public async Task Participate(CustomerDetailsRequest request, CustomerDetailsResponse response)
    {
        await Task.CompletedTask; // todo: remove

        if (request.Last5QuotesMetadata == null)
        {
            throw new Exception($"{nameof(request.Last5QuotesMetadata)} is null - execution order must be wrong");
        }

        response.Last5Quotes ??= request.Last5QuotesMetadata.Select(_ => new RatedQuoteSummary()).ToArray();

        for (var i = 0; i < response.Last5Quotes.Count; i++)
        {
            response.Last5Quotes.ElementAt(i).DateCreated = request.Last5QuotesMetadata.ElementAt(i).Added.ToString("F");

            var ratingSessionId = request.Last5QuotesMetadata.ElementAt(i).RatingSessionId;
            var completedRatingSession = grainFactory.GetGrain<IRepresentACompletedRatingSession>(ratingSessionId);
            var rates = await completedRatingSession.Ask(new GetCompletedRates());

            if (rates.Count < 1)
            {
                response.Last5Quotes.ElementAt(i).CheapestCarrier = "(Rating yet to finish)";
                response.Last5Quotes.ElementAt(i).CheapestProductName = "(Rating yet to finish)";
                response.Last5Quotes.ElementAt(i).CheapestPremium = 0;

                continue;
            }

            var cheapestRate = rates.OrderBy(x => x.Premium).First();

            response.Last5Quotes.ElementAt(i).CheapestCarrier = cheapestRate.Carrier;
            response.Last5Quotes.ElementAt(i).CheapestProductName = cheapestRate.Name;
            response.Last5Quotes.ElementAt(i).CheapestPremium = cheapestRate.Premium;
        }
    }

    public Task Rollback(Exception exception) => Task.CompletedTask;
}