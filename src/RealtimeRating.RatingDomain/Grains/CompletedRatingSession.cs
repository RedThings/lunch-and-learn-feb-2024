using Orleans.Runtime;
using RealtimeRating.RatingDomain.Dtos;
using RealtimeRating.RatingDomain.Messages;
using RealtimeRating.RatingDomain.State;

namespace RealtimeRating.RatingDomain.Grains;

public class CompletedRatingSession(
    [PersistentState(stateName: nameof(CompletedRatingSessionState), storageName: RatingDomainConstants.StorageName)]
    IPersistentState<CompletedRatingSessionState> persistentState) : IRepresentACompletedRatingSession
{
    public Task Tell(InitializeCompletedRatingSession message)
    {
        persistentState.State.NumberOfExpectedRates = message.NumberOfExpectedRates;

        return Task.CompletedTask;
    }

    public async Task Tell(CompleteRating message)
    {
        persistentState.State.Rates = message.Rates;
        persistentState.State.Completed = true;

        await persistentState.WriteStateAsync();
    }

    public Task<bool> Ask(IsRatingCompleted message) => Task.FromResult(persistentState.State.Completed);

    public Task<int> Ask(GetNumberOfExpectedRates message) => Task.FromResult(persistentState.State.NumberOfExpectedRates);

    public Task<IReadOnlyCollection<Rate>> Ask(GetCompletedRates message) => Task.FromResult(persistentState.State.Rates);
}