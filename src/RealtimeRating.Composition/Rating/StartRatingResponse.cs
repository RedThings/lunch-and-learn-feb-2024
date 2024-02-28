namespace RealtimeRating.Composition.Rating;

public class StartRatingResponse : IRepresentAComposedResponse
{
    public Guid? RatingSessionId { get; set; }
    
    // ReSharper disable once UnusedMember.Global - it is, in the UI
#pragma warning disable CA1822
    public bool RatingStarted => true;
#pragma warning restore CA1822
}