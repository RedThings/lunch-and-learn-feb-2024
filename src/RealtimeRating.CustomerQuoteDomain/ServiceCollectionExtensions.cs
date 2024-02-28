using Microsoft.Extensions.DependencyInjection;
using RealtimeRating.Composition;
using RealtimeRating.Composition.Customer;
using RealtimeRating.Composition.Rating;
using RealtimeRating.CustomerQuoteDomain.CompositionParticipators;

namespace RealtimeRating.CustomerQuoteDomain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomerQuoteDomain(this IServiceCollection services) => services
        .AddScoped<IParticipateInViewModelComposition<CustomerDetailsRequest, CustomerDetailsResponse>, CustomerDetailsParticipator>()
        .AddScoped<IParticipateInViewModelComposition<StartRatingRequest, StartRatingResponse>, StartRatingParticipator>();
}