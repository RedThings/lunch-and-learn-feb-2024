using RealtimeRating.Composition;
using RealtimeRating.Composition.RiskDataCapture;
using RealtimeRating.QuoteDomain.Grains;
using RealtimeRating.QuoteDomain.Messages;

namespace RealtimeRating.QuoteDomain.CompositionParticipators;

public class NewRiskDataCaptureParticipator(IGrainFactory grainFactory) : IParticipateInViewModelComposition<NewRiskDataCaptureRequest, NewRiskDataCaptureResponse>
{
    public int ExecutionOrder => 0;

    public async Task Participate(NewRiskDataCaptureRequest request, NewRiskDataCaptureResponse response)
    {
        var quote = grainFactory.GetGrain<IRepresentAQuote>(request.QuoteId);
        var riskVariationDetails = await quote.Ask(new GetQuoteRiskVariationDetails { Id = request.RiskVariationId });

        response.RiskVariationName = riskVariationDetails.Name;
    }

    public Task Rollback(Exception exception) => Task.CompletedTask;
}