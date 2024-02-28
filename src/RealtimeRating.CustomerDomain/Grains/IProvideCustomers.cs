using RealtimeRating.CustomerDomain.Dtos;
using RealtimeRating.CustomerDomain.Messages;

namespace RealtimeRating.CustomerDomain.Grains;

[Alias(nameof(IProvideCustomers))]
public interface IProvideCustomers : IGrainWithGuidKey
{
    [Alias(nameof(Ask) + nameof(GetCustomers))]
    Task<IReadOnlyCollection<Customer>> Ask(GetCustomers message);
}