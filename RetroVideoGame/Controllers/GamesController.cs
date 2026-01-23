using Microsoft.AspNetCore.Mvc;

public class GamesController : ControllerBase
{
    private readonly IGameService _gameService;
    private readonly List<Game> _games;

    public GamesController()
    {
        
    }

    public GamesController(GameService gameService)
    {
        _gameService = gameService;
    }

    [HttpGet("{id}")]
    public Game? getGameById(int id)
    {
        return _gameService.getGameById(id);
    }
    
    [HttpPost]
    public Game createGame(CreateGameDTO game)
    {
        return _gameService.createGame(game);
    }

    [HttpDelete("{id}")]
    public void deleteGame(int id)
    {
        _gameService.deleteGame(id);
    }

    [HttpPut("{id}")]
    public Game? updateGame(int id, UpdateGameDTO game)
    {
        return _gameService.updateGame(id, game);
    }

    [HttpPatch("{id}")]
    public Game? updatePartialGame(int id, UpdateGameDTO game)
    {
        return _gameService.updatePartialGame(id, game);
    }
}