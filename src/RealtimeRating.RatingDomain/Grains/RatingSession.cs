using Microsoft.Extensions.Logging;
using Orleans.Runtime;
using RealtimeRating.RatingDomain.Dtos;
using RealtimeRating.RatingDomain.Messages;
using RealtimeRating.RatingDomain.Messaging;
using RealtimeRating.RatingDomain.State;

namespace RealtimeRating.RatingDomain.Grains;

public class RatingSession(
    [PersistentState(stateName: nameof(RatingSessionState), storageName: RatingDomainConstants.StorageName)]
    IPersistentState<RatingSessionState> persistentState,
    ILogger<RatingSession> logger,
    /*IPublishToRedisChannels publisher*/
    IMessageSession messageSession) : Grain, IRepresentARatingSession
{
    public async Task Tell(StartRating message)
    {
        if (persistentState.State.Rates != null)
        {
            throw new Exception("Rating has already started");
        }

        persistentState.State.Rates = [];
        persistentState.State.NumberOfRatesExpected = Faker.RandomNumber.Next(0, 20);
        
        for (var i = 0; i < persistentState.State.NumberOfRatesExpected; i++)
        {
            var grainId = Guid.NewGuid();

            //await publisher.PublishAsync(new CarrierIdentified(grainId, this.GetPrimaryKey()));
            await messageSession.Publish(new CarrierIdentified(grainId, this.GetPrimaryKey()));
        }
    }

    public async Task Tell(AddRate message)
    {
        if (persistentState.State.Rates == null)
        {
            throw new Exception("Cannot persist rates - rating was never started");
        }

        persistentState.State.Rates.Add(message.Rate);
        
        if (persistentState.State.NumberOfRatesExpected > persistentState.State.Rates.Count)
        {
            return;
        }

        persistentState.State.FinishedAt = DateTime.UtcNow;

        await persistentState.WriteStateAsync();
    }
    
    public Task<FetchingRatesResult> Ask(GetFetchingRatesResult message)
    {
        if (persistentState.State.Rates == null)
        {
            logger.LogWarning("Rating never started!");

            return Task.FromResult(new FetchingRatesResult
            {
                Finished = false,
                Rates = []
            });
        }

        var result = new FetchingRatesResult
        {
            NumberOfRatesExpected = persistentState.State.NumberOfRatesExpected,
            Rates = persistentState.State.Rates,
            Finished = persistentState.State.FinishedAt != null
        };

        return Task.FromResult(result);
    }

    public async Task Tell(DeactivateRatingSession message)
    {
        persistentState.State.Rates = null;
        persistentState.State.FinishedAt = null;
        persistentState.State.NumberOfRatesExpected = 0;

        DeactivateOnIdle();

        await persistentState.ClearStateAsync();
    }
}