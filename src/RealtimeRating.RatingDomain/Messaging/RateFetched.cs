using RealtimeRating.RatingDomain.Dtos;

namespace RealtimeRating.RatingDomain.Messaging;

public record RateFetched(Rate Rate, Guid RatingSessionId) : IEvent;