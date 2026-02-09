using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("games")]
public class GamesController : ControllerBase
{
    private readonly IGameService _gameService;
    private readonly IUserService _userService;

    public GamesController(IGameService gameService)
    {
        _gameService = gameService;
    }

    [HttpGet("{id:int}")]
    public ActionResult<Game> GetGameById(int id)
    {
        var game = _gameService.GetGameById(id);
        return game is null ? NotFound() : Ok(game);
    }

    [HttpPost]
    public ActionResult<Game> CreateGame([FromBody] CreateGameDTO game)
    {
        var created = _gameService.CreateGame(game);

        if (created == null)
        return BadRequest("Game must have a valid UserId that exists.");
        
        return Created($"/games/{created.Id}", created);
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteGame(int id)
    {
        _gameService.DeleteGame(id);
        return NoContent();
    }

    [HttpPut("{id:int}")]
    public ActionResult<Game> UpdateGame(int id, [FromBody] UpdateGameDTO game)
    {
        var updated = _gameService.UpdateGame(id, game);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpPatch("{id:int}")]
    public ActionResult<Game> UpdatePartialGame(int id, [FromBody] UpdateGameDTO game)
    {
        var updated = _gameService.UpdatePartialGame(id, game);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpGet("browse/{userId:int}")]
    public ActionResult<List<Game>> BrowseGames(int userId)
    {
        if(userId == null)
        {
            return Unauthorized("Not a valid user.");
        }

        if (_userService.GetUserById(userId) == null)
        return Unauthorized("Invalid user.");

        var games = _gameService.BrowseGames(userId);
        return games is null ? NotFound() : Ok(games);
    }
}
