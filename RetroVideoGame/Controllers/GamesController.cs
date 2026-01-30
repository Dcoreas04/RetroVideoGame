using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("games")]
public class GamesController : ControllerBase
{
    private readonly IGameService _gameService;

    public GamesController(IGameService gameService)
    {
        _gameService = gameService;
    }

    [HttpGet("{id:int}")]
    public ActionResult<Game> GetGameById(int id)
    {
        var game = _gameService.getGameById(id);
        return game is null ? NotFound() : Ok(game);
    }

    [HttpPost]
    public ActionResult<Game> CreateGame([FromBody] CreateGameDTO game)
    {
        var created = _gameService.createGame(game);
        return Created($"/games/{created.Id}", created);
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteGame(int id)
    {
        _gameService.deleteGame(id);
        return NotFound();
    }

    [HttpPut("{id:int}")]
    public ActionResult<Game> UpdateGame(int id, [FromBody] UpdateGameDTO game)
    {
        var updated = _gameService.updateGame(id, game);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpPatch("{id:int}")]
    public ActionResult<Game> UpdatePartialGame(int id, [FromBody] UpdateGameDTO game)
    {
        var updated = _gameService.updatePartialGame(id, game);
        return updated is null ? NotFound() : Ok(updated);
    }
}
