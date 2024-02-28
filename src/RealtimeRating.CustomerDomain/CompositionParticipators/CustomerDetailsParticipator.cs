using RealtimeRating.Composition;
using RealtimeRating.Composition.Customer;
using RealtimeRating.CustomerDomain.Grains;
using RealtimeRating.CustomerDomain.Messages;

namespace RealtimeRating.CustomerDomain.CompositionParticipators;

public class CustomerDetailsParticipator(IGrainFactory grainFactory) : IParticipateInViewModelComposition<CustomerDetailsRequest, CustomerDetailsResponse>
{
    public int ExecutionOrder => int.MaxValue;

    public async Task Participate(CustomerDetailsRequest request, CustomerDetailsResponse response)
    {
        var customersGrain = grainFactory.GetGrain<IProvideCustomers>(Guid.Empty);

        var customers = await customersGrain.Ask(new GetCustomers());

        var customer = customers.Single(x => x.Id == request.CustomerId);

        response.CustomerLookupCode = customer.LookupCode;
        response.CustomerName = customer.Name;
    }

    public Task Rollback(Exception exception) => Task.CompletedTask;
}