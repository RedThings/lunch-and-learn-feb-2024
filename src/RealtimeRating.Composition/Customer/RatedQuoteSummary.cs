namespace RealtimeRating.Composition.Customer;

public class RatedQuoteSummary
{
    public string RiskVariationName { get; set; } = string.Empty;
    public string PolicyLineDefinitionName { get; set; } = string.Empty;
    public string CheapestCarrier { get; set; } = string.Empty;
    public string CheapestProductName { get; set; } = string.Empty;
    public decimal CheapestPremium { get; set; }
    public string DateCreated { get; set; } = string.Empty;
}