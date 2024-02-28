namespace RealtimeRating.Composition;

public interface IParticipateInViewModelComposition<in TRequest, TResponse>
    where TRequest : IRepresentARequestForComposition<TResponse>
    where TResponse : IRepresentAComposedResponse
{
    int ExecutionOrder { get; }
    Task Participate(TRequest request, TResponse response);
    Task Rollback(Exception exception);
}