using RealtimeRating.Composition;
using RealtimeRating.Composition.Rating;
using RealtimeRating.CustomerQuoteDomain.Grains;
using RealtimeRating.CustomerQuoteDomain.Messages;

namespace RealtimeRating.CustomerQuoteDomain.CompositionParticipators;

public class StartRatingParticipator(IGrainFactory grainFactory) : IParticipateInViewModelComposition<StartRatingRequest, StartRatingResponse>
{
    private StartRatingRequest? participateRequest;

    public int ExecutionOrder => int.MaxValue;

    public async Task Participate(StartRatingRequest request, StartRatingResponse response)
    {
        if (request.DoNotRate)
        {
            return;
        }

        if (response.RatingSessionId == null)
        {
            throw new Exception(
                $"Could not call {nameof(AddCustomerQuoteMetadata)} - "
                + $"{nameof(StartRatingResponse)}.{nameof(StartRatingResponse.RatingSessionId)} "
                + $"is null at this stage");
        }

        participateRequest = request;

        var grain = grainFactory.GetGrain<ITrackQuotesForACustomer>(request.CustomerId);

        await grain.Tell(
            new AddCustomerQuoteMetadata
            {
                PolicyLineDefinitionCode = request.PolicyLineDefinitionCode,
                QuoteId = request.QuoteId,
                RatingSessionId = response.RatingSessionId.Value,
                RiskVariationId = request.RiskVariationId
            });
    }

    public async Task Rollback(Exception exception)
    {
        if (participateRequest == null)
        {
            return;
        }

        var grain = grainFactory.GetGrain<ITrackQuotesForACustomer>(participateRequest.CustomerId);

        await grain.Tell(new RemoveCustomerQuoteMetadata
        {
            RiskVariationId = participateRequest.RiskVariationId
        });
    }
}