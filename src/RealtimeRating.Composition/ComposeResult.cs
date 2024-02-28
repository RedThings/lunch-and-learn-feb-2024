namespace RealtimeRating.Composition;

public class ComposeResult<TResponse> where TResponse : IRepresentAComposedResponse
{
    public bool Success => SuccessResponse != null && FailureResponse == null;
    public TResponse? SuccessResponse { get; init; }
    public ComposeResultFailure? FailureResponse { get; init; }
}