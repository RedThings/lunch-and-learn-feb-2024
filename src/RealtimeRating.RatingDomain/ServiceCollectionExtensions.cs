using Microsoft.Extensions.DependencyInjection;
using RealtimeRating.Composition;
using RealtimeRating.Composition.Customer;
using RealtimeRating.Composition.Rating;
using RealtimeRating.RatingDomain.CompositionParticipators;

namespace RealtimeRating.RatingDomain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRatingDomain(this IServiceCollection services)
    {
        services
            .AddScoped<IParticipateInViewModelComposition<CustomerDetailsRequest, CustomerDetailsResponse>, CustomerDetailsParticipator>()
            .AddScoped<IParticipateInViewModelComposition<StartRatingRequest, StartRatingResponse>, StartRatingParticipator>()
            .AddScoped<IParticipateInViewModelComposition<DisplayRatingResultsRequest, DisplayRatingResultsResponse>, DisplayRatingResultsParticipator>();

        return services;
    }
}