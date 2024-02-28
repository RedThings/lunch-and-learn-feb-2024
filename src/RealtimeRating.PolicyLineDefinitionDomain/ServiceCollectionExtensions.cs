using Microsoft.Extensions.DependencyInjection;
using RealtimeRating.Composition;
using RealtimeRating.Composition.Customer;
using RealtimeRating.Composition.NewQuote;
using RealtimeRating.Composition.Rating;
using RealtimeRating.Composition.RiskDataCapture;
using RealtimeRating.PolicyLineDefinitionDomain.CompositionParticipators;

namespace RealtimeRating.PolicyLineDefinitionDomain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPolicyLineDefinitionDomain(this IServiceCollection services) => services
        .AddScoped<IParticipateInViewModelComposition<CustomerDetailsRequest, CustomerDetailsResponse>, CustomerDetailsParticipator>()
        .AddScoped<IParticipateInViewModelComposition<NewQuoteRequest, NewQuoteResponse>, NewQuoteParticipator>()
        .AddScoped<IParticipateInViewModelComposition<NewRiskDataCaptureRequest, NewRiskDataCaptureResponse>, NewRiskDataCaptureParticipator>()
        .AddScoped<IParticipateInViewModelComposition<SubmitNewRiskDataCaptureRequest, SubmitNewRiskDataCaptureResponse>, SubmitNewRiskDataCaptureParticipator>()
        .AddScoped<IParticipateInViewModelComposition<DisplayRatingResultsRequest, DisplayRatingResultsResponse>, DisplayRatingResultsParticipator>();
}