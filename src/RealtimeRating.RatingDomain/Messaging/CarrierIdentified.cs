namespace RealtimeRating.RatingDomain.Messaging;

public record CarrierIdentified(Guid CarrierConnectorId, Guid RatingSessionId) : IEvent;