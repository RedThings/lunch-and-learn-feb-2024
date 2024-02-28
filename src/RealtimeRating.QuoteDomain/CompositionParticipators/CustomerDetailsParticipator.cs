using RealtimeRating.Composition;
using RealtimeRating.Composition.Customer;
using RealtimeRating.QuoteDomain.Grains;
using RealtimeRating.QuoteDomain.Messages;

namespace RealtimeRating.QuoteDomain.CompositionParticipators;

public class CustomerDetailsParticipator(IGrainFactory grainFactory) : IParticipateInViewModelComposition<CustomerDetailsRequest, CustomerDetailsResponse>
{
    public int ExecutionOrder => 0;

    public async Task Participate(CustomerDetailsRequest request, CustomerDetailsResponse response)
    {
        if (request.Last5QuotesMetadata == null)
        {
            throw new Exception($"{nameof(request.Last5QuotesMetadata)} is null - execution order must be wrong");
        }

        response.Last5Quotes ??= request.Last5QuotesMetadata.Select(_ => new RatedQuoteSummary()).ToArray();

        for (var i = 0; i < response.Last5Quotes.Count; i++)
        {
            var quoteId = request.Last5QuotesMetadata.ElementAt(i).QuoteId;
            var riskVariationId = request.Last5QuotesMetadata.ElementAt(i).RiskVariationId;

            var quote = grainFactory.GetGrain<IRepresentAQuote>(quoteId);
            var riskVariation = await quote.Ask(new GetQuoteRiskVariationDetails { Id = riskVariationId });

            response.Last5Quotes.ElementAt(i).RiskVariationName = riskVariation.Name;
        }
    }

    public Task Rollback(Exception exception) => Task.CompletedTask;
}