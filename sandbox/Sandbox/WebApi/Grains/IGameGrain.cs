using Orleans;

namespace WebApi.Grains;

public interface IGameGrain : IGrainWithGuidKey
{
    Task StartGame(string name);
    Task<string?> GetName();
}