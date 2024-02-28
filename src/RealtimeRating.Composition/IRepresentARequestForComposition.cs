namespace RealtimeRating.Composition;

// ReSharper disable once UnusedTypeParameter
public interface IRepresentARequestForComposition<TResponse>
    where TResponse : IRepresentAComposedResponse;