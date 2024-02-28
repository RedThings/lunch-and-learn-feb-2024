using Orleans.Runtime;
using RealtimeRating.CustomerQuoteDomain.Dtos;
using RealtimeRating.CustomerQuoteDomain.Messages;
using RealtimeRating.CustomerQuoteDomain.State;

namespace RealtimeRating.CustomerQuoteDomain.Grains;

public class CustomerQuoteTracker(
    [PersistentState(stateName: nameof(CustomerQuoteTrackingState), storageName: CustomerQuoteDomainConstants.StorageName)]
    IPersistentState<CustomerQuoteTrackingState> persistentState
    ) : Grain, ITrackQuotesForACustomer
{
    public async Task Tell(AddCustomerQuoteMetadata message)
    {
        var existing = persistentState.State.CustomerQuoteMetadatas.SingleOrDefault(x => x.RiskVariationId == message.RiskVariationId);

        if (existing != null)
        {
            return;
        }

        var metadata = new CustomerQuoteMetadata
        {
            PolicyLineDefinitionCode = message.PolicyLineDefinitionCode,
            QuoteId = message.QuoteId,
            RiskVariationId = message.RiskVariationId,
            RatingSessionId = message.RatingSessionId,
            Added = DateTime.UtcNow
        };

        persistentState.State.CustomerQuoteMetadatas.Add(metadata);

        await persistentState.WriteStateAsync();
    }

    public async Task Tell(RemoveCustomerQuoteMetadata message)
    {
        var found = persistentState.State.CustomerQuoteMetadatas.SingleOrDefault(x => x.RiskVariationId == message.RiskVariationId);

        if (found == null)
        {
            return;
        }

        persistentState.State.CustomerQuoteMetadatas.Remove(found);

        await persistentState.WriteStateAsync();
    }

    public Task<IReadOnlyCollection<CustomerQuoteMetadata>> Ask(GetLast5Quotes message)
    {
        if (persistentState.State.CustomerQuoteMetadatas.Count < 1)
        {
            var output = Array.Empty<CustomerQuoteMetadata>();
            return Task.FromResult((IReadOnlyCollection<CustomerQuoteMetadata>) output);
        }

        var quotes = persistentState.State.CustomerQuoteMetadatas.OrderByDescending(x => x.Added).Take(5).ToArray();

        return Task.FromResult((IReadOnlyCollection<CustomerQuoteMetadata>) quotes);
    }
}