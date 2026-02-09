public class GameService : IGameService
    {
    private static readonly List<Game> _games = new List<Game>();
    private readonly IUserService _userService;

private static int _nextGameId = 1;

public GameService(IUserService userService)
    {
        _userService = userService;
    }

public Game? CreateGame(CreateGameDTO gameDTO)
{
    if (gameDTO.UserId == null)
    {
        return null;
    }

    var userId = gameDTO.UserId.Value;

    var user = _userService.GetUserById(userId);
    if (user == null)
    {
        return null;
    }

    var game = new Game
    {
        Id = _nextGameId++,
        Title = gameDTO.Title,
        Publisher = gameDTO.Publisher,
        ReleaseDate = gameDTO.ReleaseDate,
        Platform = gameDTO.Platform,
        Condition = gameDTO.Condition,
        UserId = userId
    };

    _games.Add(game);
    return game;
}

        

    public Game? GetGameById(int id)
    {
        return _games.FirstOrDefault(g => g.Id == id);
    }

    public void DeleteGame(int id)
    {
        var game = GetGameById(id);
        if (game == null)
        {
            return;
        }

        _games.Remove(game);
    }

    public Game? UpdateGame(int id, UpdateGameDTO game)
    {
        var gameToUpdate = GetGameById(id);
        if (gameToUpdate == null)
        {
            return null;
        }

        gameToUpdate.Condition = (Conditions)game.Condition;

        return gameToUpdate;
    }

    public Game? UpdatePartialGame(int id, UpdateGameDTO game)
    {
        var gameToUpdate = GetGameById(id);
        if (gameToUpdate == null)
        {
            return null;
        }

        if (game.Condition != null)
        {
            gameToUpdate.Condition = (Conditions)game.Condition;
        }

        if(game.Title != null)
        {
            gameToUpdate.Title = game.Title;
        }

        return gameToUpdate;
    }

    public List<Game> BrowseGames(int userId)
    {
        return _games.Where(g => g.UserId != userId).ToList();
    }
}