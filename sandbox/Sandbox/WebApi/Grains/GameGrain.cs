using Orleans;

namespace WebApi.Grains;

public class GameGrain : Grain, IGameGrain
{
    private string? nameOfGame;

    public Task StartGame(string name)
    {
        nameOfGame = name;

        return Task.CompletedTask;
    }

    public Task<string?> GetName() => Task.FromResult(nameOfGame);
}