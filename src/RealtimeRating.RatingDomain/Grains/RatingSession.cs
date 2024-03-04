using Orleans.Runtime;
using RealtimeRating.RatingDomain.Dtos;
using RealtimeRating.RatingDomain.Messages;
using RealtimeRating.RatingDomain.State;

namespace RealtimeRating.RatingDomain.Grains;

public class RatingSession(
    [PersistentState(stateName: nameof(RatingSessionState), storageName: RatingDomainConstants.StorageName)]
    IPersistentState<RatingSessionState> persistentState
    /*IPublishToRedisChannels publisher*/
    /*IMessageSession messageSession*/) : Grain, IRepresentARatingSession
{
    private IDisposable? startRatingTimer;

    public async Task Tell(StartRating message)
    {
        persistentState.State.NumberOfRatesExpected = Faker.RandomNumber.Next(20, 100);

        var completedGrain = GrainFactory.GetGrain<IRepresentACompletedRatingSession>(this.GetPrimaryKey());

        await completedGrain.Tell(new InitializeCompletedRatingSession
        {
            NumberOfExpectedRates = persistentState.State.NumberOfRatesExpected
        });

        startRatingTimer = RegisterTimer(OnStartRating, new object(), TimeSpan.FromMilliseconds(50), TimeSpan.FromSeconds(1));
    }

    private async Task OnStartRating(object _)
    {
        startRatingTimer?.Dispose();

        var rates = new Rate[persistentState.State.NumberOfRatesExpected];

        for (var i = 0; i < persistentState.State.NumberOfRatesExpected; i++)
        {
            var premium = decimal.Parse($"{Faker.RandomNumber.Next(0, 600)}.{Faker.RandomNumber.Next(0, 99)}");

            var rate = new Rate
            {
                Carrier = Faker.Company.Name(),
                Name = Faker.Company.BS(),
                Premium = premium
            };

            rates[i] = rate;
        }

        // faking a Task.WhenAll calling rating providers
        await Task.Delay(Faker.RandomNumber.Next(500, 2000));
        //

        var completedGrain = GrainFactory.GetGrain<IRepresentACompletedRatingSession>(this.GetPrimaryKey());

        await completedGrain.Tell(new CompleteRating
        {
            Rates = rates
        });
        
        await persistentState.WriteStateAsync();
    }

    public async Task Tell(DeactivateRatingSession message)
    {
        DeactivateOnIdle();

        await persistentState.ClearStateAsync();
    }
}