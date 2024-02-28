namespace RealtimeRating.Composition;

public interface IComposeGrains
{
    Task<ComposeResult<TResponse>> Compose<TRequest, TResponse>(TRequest request)
        where TRequest : IRepresentARequestForComposition<TResponse>
        where TResponse : IRepresentAComposedResponse;

    Task<ComposeResult<TResponse>> Compose<TRequest, TResponse>()
        where TRequest : IRepresentARequestForComposition<TResponse>
        where TResponse : IRepresentAComposedResponse;
}