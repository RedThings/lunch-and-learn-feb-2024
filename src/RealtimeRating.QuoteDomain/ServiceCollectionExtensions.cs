using Microsoft.Extensions.DependencyInjection;
using RealtimeRating.Composition;
using RealtimeRating.Composition.Customer;
using RealtimeRating.Composition.NewQuote;
using RealtimeRating.Composition.Rating;
using RealtimeRating.Composition.RiskDataCapture;
using RealtimeRating.QuoteDomain.CompositionParticipators;

namespace RealtimeRating.QuoteDomain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQuoteDomain(this IServiceCollection services) => services
        .AddScoped<IParticipateInViewModelComposition<CustomerDetailsRequest, CustomerDetailsResponse>, CustomerDetailsParticipator>()
        .AddScoped<IParticipateInViewModelComposition<SubmitNewQuoteRequest, SubmitNewQuoteResponse>, SubmitNewQuoteParticipator>()
        .AddScoped<IParticipateInViewModelComposition<NewRiskDataCaptureRequest, NewRiskDataCaptureResponse>, NewRiskDataCaptureParticipator>()
        .AddScoped<IParticipateInViewModelComposition<DisplayRatingResultsRequest, DisplayRatingResultsResponse>, DisplayRatingResultsParticipator>();
}