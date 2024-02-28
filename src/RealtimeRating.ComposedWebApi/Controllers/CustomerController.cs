using Microsoft.AspNetCore.Mvc;
using RealtimeRating.Composition;
using RealtimeRating.Composition.Customer;

namespace RealtimeRating.ComposedWebApi.Controllers;

public class CustomerController(IComposeGrains grainComposer) : ControllerBase
{
    [HttpGet("/customer")]
    public async Task<IActionResult> Get([FromQuery(Name = "customer_id")] Guid customerId)
    {
        var result = await grainComposer.Compose<CustomerDetailsRequest, CustomerDetailsResponse>(new CustomerDetailsRequest
        {
            CustomerId = customerId
        });

        return result.Success ? Ok(result.SuccessResponse) : StatusCode(502, result.FailureResponse);
    }
}