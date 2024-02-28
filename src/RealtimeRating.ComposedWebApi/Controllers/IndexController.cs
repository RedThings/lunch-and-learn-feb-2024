using Microsoft.AspNetCore.Mvc;
using RealtimeRating.CustomerDomain.Grains;
using RealtimeRating.CustomerDomain.Messages;

namespace RealtimeRating.ComposedWebApi.Controllers;

public class IndexController(IGrainFactory grainFactory) : ControllerBase
{
    [HttpGet("/")]
    public async Task<IActionResult> Get()
    {
        var customersGrain = grainFactory.GetGrain<IProvideCustomers>(Guid.Empty);

        var customers = await customersGrain.Ask(new GetCustomers());

        // no need for composition here so don't overcomplicate it
        return Ok(customers);
    }
}