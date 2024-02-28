using Microsoft.AspNetCore.Mvc;
using Orleans;
using WebApi.Grains;

namespace WebApi.Controllers
{
    [ApiController]
    public class GameController(IGrainFactory grainFactory) : ControllerBase
    {
        [Route("game/start")]
        public async Task<IActionResult> StartGame([FromQuery] string name)
        {
            var gameId = Guid.NewGuid();

            var gameGrain = grainFactory.GetGrain<IGameGrain>(gameId);

            await gameGrain.StartGame(name);

            return Ok(gameId);
        }

        [Route("game/{id:guid}")]
        public async Task<IActionResult> StartGame([FromRoute] Guid id)
        {
            var gameGrain = grainFactory.GetGrain<IGameGrain>(id);

            var name = await gameGrain.GetName();

            return Ok(name);
        }
    }
}
