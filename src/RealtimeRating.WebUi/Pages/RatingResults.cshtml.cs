using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RealtimeRating.WebUi.Pages;

public class RatingResultsModel : PageModel
{
    public Guid CustomerId { get; private set; }
    public Guid QuoteId { get; private set; }
    public Guid RiskVariationId { get; private set; }
    public string PolicyLineDefinitionCode { get; private set; } = string.Empty;
    public Guid RatingSessionId { get; private set; }

    public void OnGet()
    {
        CustomerId = Guid.Parse(Request.Query["customer_id"].Single() ?? string.Empty);
        QuoteId = Guid.Parse(Request.Query["quote_id"].Single() ?? string.Empty);
        RiskVariationId = Guid.Parse(Request.Query["risk_variation_id"].Single() ?? string.Empty);
        PolicyLineDefinitionCode = Request.Query["policy_line_definition_code"].Single() ?? string.Empty;
        RatingSessionId = Guid.Parse(Request.Query["rating_session_id"].Single() ?? string.Empty);
    }
}