using RealtimeRating.Composition;
using RealtimeRating.Composition.Rating;
using RealtimeRating.RatingDomain.Grains;
using RealtimeRating.RatingDomain.Messages;

namespace RealtimeRating.RatingDomain.CompositionParticipators;

//public class StartRatingParticipator(IGrainFactory grainFactory)
//public class StartRatingParticipator(IMessageSession messageSession)
public class StartRatingParticipator(IGrainFactory grainFactory) : IParticipateInViewModelComposition<StartRatingRequest, StartRatingResponse>
{
    private IRepresentARatingSession? session;

    public int ExecutionOrder => 0;

    public async Task Participate(StartRatingRequest request, StartRatingResponse response)
    {
        var ratingSessionId = Guid.NewGuid();
        response.RatingSessionId = ratingSessionId;

        if (request.DoNotRate)
        {
            return;
        }

        //await messageSession.Publish(new RatingRequested(ratingSessionId));
        
        session = grainFactory.GetGrain<IRepresentARatingSession>(ratingSessionId);

        await session.Tell(new StartRating());
    }

    public async Task Rollback(Exception exception)
    {
        if (session == null)
        {
            return;
        }

        await session.Tell(new DeactivateRatingSession());
    }
}