using Microsoft.Extensions.DependencyInjection;
using RealtimeRating.Composition;
using RealtimeRating.Composition.Customer;
using RealtimeRating.CustomerDomain.CompositionParticipators;

namespace RealtimeRating.CustomerDomain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomerDomain(this IServiceCollection services) => services
        .AddScoped<IParticipateInViewModelComposition<CustomerDetailsRequest, CustomerDetailsResponse>, CustomerDetailsParticipator>();
}