using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace RealtimeRating.Composition;

public class GrainComposer(IServiceProvider serviceProvider) : IComposeGrains
{
    public async Task<ComposeResult<TResponse>> Compose<TRequest, TResponse>(TRequest request)
        where TRequest : IRepresentARequestForComposition<TResponse>
        where TResponse : IRepresentAComposedResponse
    {
        var logger = serviceProvider.GetRequiredService<ILogger<GrainComposer>>();
        var services = serviceProvider.GetServices<IParticipateInViewModelComposition<TRequest, TResponse>>().ToArray();

        if (services.Length < 1)
        {
            throw new InvalidOperationException($"{nameof(GrainComposer)} requires at least one participating service");
        }

        var response = Activator.CreateInstance<TResponse>();

        var exceptions = new List<Exception>();
        var rollbackExceptions = new List<Exception>();

        foreach (var service in services.OrderBy(x => x.ExecutionOrder)) // todo: parallel? would then need to merge results...
        {
            try
            {
                await service.Participate(request, response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);

                logger.LogError(
                    "An exception occured in the participating service '{name}': {ex}. Stack trace: {stack}", 
                    service.GetType().FullName, 
                    ex.Message,
                    ex.StackTrace
                );

                try
                {
                    await service.Rollback(ex).ConfigureAwait(false);
                }
                catch (Exception rollbackException)
                {
                    logger.LogError(
                        "An exception occured during rollback in the participating service '{name}': {ex}. Stack trace: {stack}", 
                        service.GetType().FullName, 
                        rollbackException.Message,
                        rollbackException.StackTrace
                    );

                    rollbackExceptions.Add(rollbackException);
                }
            }
        }

        if (exceptions.Count > 0)
        {
            return new ComposeResult<TResponse>
            {
                FailureResponse = ComposeResultFailure.Create(exceptions, rollbackExceptions)
            };
        }

        return new ComposeResult<TResponse>
        {
            SuccessResponse = response
        };
    }

    public Task<ComposeResult<TResponse>> Compose<TRequest, TResponse>()
        where TRequest : IRepresentARequestForComposition<TResponse> where TResponse : IRepresentAComposedResponse =>
        Compose<TRequest, TResponse>(Activator.CreateInstance<TRequest>());
}