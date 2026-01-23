public class GameService : IGameService
    {
    private readonly List<Game> _games = new List<Game>();
    public Game createGame(CreateGameDTO gameDTO)
    {

        var games = new Game
        {
            Id = gameDTO.Id,
            Title = gameDTO.Title,
            Publisher = gameDTO.Publisher,
            ReleaseDate = gameDTO.ReleaseDate,
            Platform = gameDTO.Console,
            Condition = (Conditions)gameDTO.Condition
        };

        _games.Add(games);
        return games;
    }

    public Game? getGameById(int id)
    {
        return _games.FirstOrDefault(g => g.Id == id);
    }
    public void deleteGame(int id)
    {
        var game = getGameById(id);
        if (game == null)
        {
            return;
        }

        _games.Remove(game);
    }

    public Game? updateGame(int id, UpdateGameDTO game)
    {
        var gameToUpdate = getGameById(id);
        if (gameToUpdate == null)
        {
            return null;
        }

        gameToUpdate.Condition = (Conditions)game.Condition;

        return gameToUpdate;
    }

    public Game? updatePartialGame(int id, UpdateGameDTO game)
    {
        throw new NotImplementedException();
    }
}