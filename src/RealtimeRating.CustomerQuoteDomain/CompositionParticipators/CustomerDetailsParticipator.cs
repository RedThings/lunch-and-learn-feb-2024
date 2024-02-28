using RealtimeRating.Composition;
using RealtimeRating.Composition.Customer;
using RealtimeRating.CustomerQuoteDomain.Grains;
using RealtimeRating.CustomerQuoteDomain.Messages;

namespace RealtimeRating.CustomerQuoteDomain.CompositionParticipators;

public class CustomerDetailsParticipator(IGrainFactory grainFactory) : IParticipateInViewModelComposition<CustomerDetailsRequest, CustomerDetailsResponse>
{
    public int ExecutionOrder => int.MinValue;

    public async Task Participate(CustomerDetailsRequest request, CustomerDetailsResponse response)
    {
        var customerQuoteGrain = grainFactory.GetGrain<ITrackQuotesForACustomer>(request.CustomerId);

        var last5Quotes = await customerQuoteGrain.Ask(new GetLast5Quotes());

        request.Last5QuotesMetadata = last5Quotes.Select(x => new QuoteMetadataForCustomerDetailsRequest
        {
            QuoteId = x.QuoteId,
            PolicyLineDefinitionCode = x.PolicyLineDefinitionCode,
            RiskVariationId = x.RiskVariationId,
            RatingSessionId = x.RatingSessionId,
            Added = x.Added
        }).ToArray();
    }

    public Task Rollback(Exception exception) => Task.CompletedTask;
}