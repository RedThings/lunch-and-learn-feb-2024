using Microsoft.AspNetCore.Mvc;
using RealtimeRating.Composition;
using RealtimeRating.Composition.NewQuote;

namespace RealtimeRating.ComposedWebApi.Controllers;

public class NewQuoteController(IComposeGrains grainComposer) : Controller
{
    [HttpGet("/new-quote")]
    public async Task<IActionResult> Get()
    {
        var result = await grainComposer.Compose<NewQuoteRequest, NewQuoteResponse>();

        return result.Success ? Ok(result.SuccessResponse) : StatusCode(502, result.FailureResponse);
    }

    [HttpPost("/new-quote")]
    public async Task<IActionResult> Post([FromBody] SubmitNewQuoteRequest request)
    {
        var result = await grainComposer.Compose<SubmitNewQuoteRequest, SubmitNewQuoteResponse>(request);

        return result.Success ? Ok(result.SuccessResponse) : StatusCode(502, result.FailureResponse);
    }
}