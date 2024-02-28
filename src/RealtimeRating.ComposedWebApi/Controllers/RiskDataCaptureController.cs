using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RealtimeRating.Composition;
using RealtimeRating.Composition.Rating;
using RealtimeRating.Composition.RiskDataCapture;

namespace RealtimeRating.ComposedWebApi.Controllers;

public class RiskDataCaptureController(IComposeGrains grainComposer) : Controller
{
    [HttpGet("/risk-data-capture")]
    public async Task<IActionResult> Get(
        [FromQuery(Name = "customer_id")] Guid customerId,
        [FromQuery(Name = "quote_id")] Guid quoteId,
        [FromQuery(Name = "risk_variation_id")] Guid riskVariationId,
        [FromQuery(Name = "policy_line_definition_code")] string policyLineDefinitionCode)
    {
        var request = new NewRiskDataCaptureRequest
        {
            QuoteId = quoteId,
            RiskVariationId = riskVariationId,
            PolicyLineDefinitionCode = policyLineDefinitionCode
        };

        var result = await grainComposer.Compose<NewRiskDataCaptureRequest, NewRiskDataCaptureResponse>(request);

        return result.Success ? Ok(result.SuccessResponse) : StatusCode(502, result.FailureResponse);
    }

    [HttpPost("/risk-data-capture")]
    public async Task<IActionResult> Post(
        [FromQuery(Name = "customer_id")] Guid customerId,
        [FromQuery(Name = "quote_id")] Guid quoteId,
        [FromQuery(Name = "risk_variation_id")] Guid riskVariationId,
        [FromQuery(Name = "policy_line_definition_code")] string policyLineDefinitionCode,
        [FromQuery(Name = "do_not_rate")] bool doNotRate,
        [FromForm] IFormCollection form)
    {
        var request = new SubmitNewRiskDataCaptureRequest
        {
            QuoteId = quoteId,
            RiskVariationId = riskVariationId,
            PolicyLineDefinitionCode = policyLineDefinitionCode,
            Answers = form.Select(x => new KeyValuePair<string, string?>(x.Key, x.Value)).ToArray()
        };
        
        var result = await grainComposer.Compose<SubmitNewRiskDataCaptureRequest, SubmitNewRiskDataCaptureResponse>(request);

        if (!result.Success)
        {
            return StatusCode(502, result.FailureResponse);
        }

        if (result.SuccessResponse is { Valid: false })
        {
            return Accepted(result.SuccessResponse); // should be 422 but makes UI more complex
        }

        var startRatingResult = await grainComposer.Compose<StartRatingRequest, StartRatingResponse>(new StartRatingRequest
        {
            PolicyLineDefinitionCode = policyLineDefinitionCode,
            RiskData = JsonSerializer.Serialize(form),
            CustomerId = customerId,
            QuoteId = quoteId,
            RiskVariationId = riskVariationId,
            DoNotRate = doNotRate
        });

        return startRatingResult.Success ? Ok(startRatingResult.SuccessResponse) : StatusCode(502, startRatingResult.FailureResponse);
    }
}