using Microsoft.Extensions.DependencyInjection;

namespace RealtimeRating.Composition;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGrainComposer(this IServiceCollection services)
    {
        return services.AddScoped<IComposeGrains>(sp => new GrainComposer(sp));
    }
}