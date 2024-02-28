using RealtimeRating.Composition;
using RealtimeRating.Composition.NewQuote;
using RealtimeRating.QuoteDomain.Grains;
using RealtimeRating.QuoteDomain.Messages;

namespace RealtimeRating.QuoteDomain.CompositionParticipators;

public class SubmitNewQuoteParticipator(IGrainFactory grainFactory) : IParticipateInViewModelComposition<SubmitNewQuoteRequest, SubmitNewQuoteResponse>
{
    private IRepresentAQuote? quote;

    public int ExecutionOrder => 0;

    public async Task Participate(SubmitNewQuoteRequest request, SubmitNewQuoteResponse response)
    {
        var quoteId = Guid.NewGuid();
        var riskVariationId = Guid.NewGuid();

        quote = grainFactory.GetGrain<IRepresentAQuote>(quoteId);

        await quote.Tell(new AddRiskVariation
        {
            Name = request.Name,
            Id = riskVariationId
        });

        response.QuoteId = quoteId;
        response.RiskVariationId = riskVariationId;
    }

    public async Task Rollback(Exception exception)
    {
        if (quote == null)
        {
            return;
        }

        await quote.Tell(new DeactivateQuote());
    }
}