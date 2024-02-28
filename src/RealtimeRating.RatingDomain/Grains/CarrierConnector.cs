using RealtimeRating.RatingDomain.Dtos;
using RealtimeRating.RatingDomain.Messages;
using RealtimeRating.RatingDomain.Messaging;

namespace RealtimeRating.RatingDomain.Grains;

public class CarrierConnector( /*IPublishToRedisChannels publisher*/ IMessageSession messageSession) : Grain, IConnectToACarrier
{
    public async Task Tell(FetchRatesFromCarrier message)
    {
        // ! this represents a call using HttpClient or whatever to get rates from a provider ! //
        var delay = Faker.RandomNumber.Next(500, 1000);
        await Task.Delay(delay);
        // ! 

        var carrier = Faker.Company.Name();

        var premium = decimal.Parse($"{Faker.RandomNumber.Next(100, 3000)}.{Faker.RandomNumber.Next(10, 99)}");

        var rate = new Rate
        {
            Carrier = carrier,
            Name = Faker.Company.CatchPhrase(),
            Premium = premium
        };

        // await publisher.PublishAsync(new RateFetched(rate, message.RatingSessionId));
        await messageSession.Publish(new RateFetched(rate, message.RatingSessionId));
    }
}