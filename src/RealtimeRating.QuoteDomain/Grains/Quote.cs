using Orleans.Runtime;
using RealtimeRating.QuoteDomain.Dtos;
using RealtimeRating.QuoteDomain.Messages;
using RealtimeRating.QuoteDomain.State;

namespace RealtimeRating.QuoteDomain.Grains;

public class Quote(
    [PersistentState(stateName: nameof(QuoteState), storageName: QuoteDomainConstants.StorageName)]
    IPersistentState<QuoteState> persistentState
    ) : Grain, IRepresentAQuote
{
    public async Task Tell(AddRiskVariation message)
    {
        var riskVariation = persistentState.State.RiskVariations.SingleOrDefault(x => x.Id == message.Id);

        if (riskVariation != null)
        {
            return;
        }

        persistentState.State.RiskVariations.Add(new RiskVariationState { Id = message.Id, Name = message.Name });

        await persistentState.WriteStateAsync();
    }

    public async Task Tell(DeactivateQuote _)
    {
        persistentState.State.RiskVariations = [];

        DeactivateOnIdle();

        await persistentState.ClearStateAsync();
    }

    public Task<QuoteRiskVariationDetails> Ask(GetQuoteRiskVariationDetails message)
    {
        var riskVariation = persistentState.State.RiskVariations.SingleOrDefault(x => x.Id == message.Id);

        if (riskVariation == null)
        {
            throw new Exception($"Risk variation {message.Id} does not exist");
        }

        return Task.FromResult(new QuoteRiskVariationDetails
        {
            Name = riskVariation.Name
        });
    }
}