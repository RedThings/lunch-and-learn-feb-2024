using RealtimeRating.QuoteDomain.Dtos;
using RealtimeRating.QuoteDomain.Messages;

namespace RealtimeRating.QuoteDomain.Grains;

[Alias(nameof(IRepresentAQuote))]
public interface IRepresentAQuote : IGrainWithGuidKey
{
    [Alias(nameof(Tell) + nameof(AddRiskVariation))]
    Task Tell(AddRiskVariation message);

    [Alias(nameof(Tell) + nameof(DeactivateQuote))]
    Task Tell(DeactivateQuote _);

    [Alias(nameof(Ask) + nameof(GetQuoteRiskVariationDetails))]
    Task<QuoteRiskVariationDetails> Ask(GetQuoteRiskVariationDetails message);
}