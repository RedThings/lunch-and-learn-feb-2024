using RealtimeRating.CustomerQuoteDomain.Dtos;
using RealtimeRating.CustomerQuoteDomain.Messages;

namespace RealtimeRating.CustomerQuoteDomain.Grains;

[Alias(nameof(ITrackQuotesForACustomer))]
public interface ITrackQuotesForACustomer : IGrainWithGuidKey
{
    [Alias(nameof(Tell) + nameof(AddCustomerQuoteMetadata))]
    Task Tell(AddCustomerQuoteMetadata message);

    [Alias(nameof(Tell) + nameof(RemoveCustomerQuoteMetadata))]
    Task Tell(RemoveCustomerQuoteMetadata message);

    [Alias(nameof(Ask) + nameof(GetLast5Quotes))]
    Task<IReadOnlyCollection<CustomerQuoteMetadata>> Ask(GetLast5Quotes message);
}