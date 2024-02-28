using RealtimeRating.CustomerDomain.Dtos;
using RealtimeRating.CustomerDomain.Messages;

namespace RealtimeRating.CustomerDomain.Grains;

public class CustomersProvider : Grain, IProvideCustomers
{
    private IReadOnlyCollection<Customer>? users;

    public Task<IReadOnlyCollection<Customer>> Ask(GetCustomers message) => users != null ? Task.FromResult(users) : SetUsers();

    private Task<IReadOnlyCollection<Customer>> SetUsers()
    {
        var ids = new[]
        {
            new Guid("4e89378b-412a-4414-be1a-5eebed5347f3"),
            new Guid("a5d3cc3d-8d89-4c36-afbd-d453743a8d0a"),
            new Guid("50e192dd-5745-4024-a03f-fa983bdcc65d"),
            new Guid("6a3181cf-8202-4be1-8855-546730198a85"),
            new Guid("48a22cdb-b6fd-4587-b56a-9976467fbba4")
        };

        users = ids.Select(id =>
        {
            var name = Faker.Name.FullName();

            return new Customer
            {
                Id = id,
                LookupCode = Faker.Internet.UserName(name),
                Name = name
            };
        }).ToArray();

        return Task.FromResult(users);
    }
}