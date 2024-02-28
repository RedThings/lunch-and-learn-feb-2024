using Microsoft.AspNetCore.Mvc;
using RealtimeRating.Composition;
using RealtimeRating.Composition.Rating;

namespace RealtimeRating.ComposedWebApi.Controllers;

public class RatingResultsController(IComposeGrains grainComposer) : Controller
{
    [HttpGet("/rating-results")]
    public async Task<IActionResult> Get(
        [FromQuery(Name = "customer_id")] Guid customerId,
        [FromQuery(Name = "quote_id")] Guid quoteId,
        [FromQuery(Name = "risk_variation_id")] Guid riskVariationId,
        [FromQuery(Name = "policy_line_definition_code")] string policyLineDefinitionCode,
        [FromQuery(Name = "rating_session_id")] Guid ratingSessionId)
    {
        var request = new DisplayRatingResultsRequest
        {
            CustomerId = customerId,
            QuoteId = quoteId,
            RiskVariationId = riskVariationId,
            PolicyLineDefinitionCode = policyLineDefinitionCode,
            RatingSessionId = ratingSessionId
        };

        var result = await grainComposer.Compose<DisplayRatingResultsRequest, DisplayRatingResultsResponse>(request);
        
        return result.Success ? Ok(result.SuccessResponse) : StatusCode(502, result.FailureResponse);
    }
}