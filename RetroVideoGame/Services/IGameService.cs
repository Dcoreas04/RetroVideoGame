public interface IGameService
{
    Game CreateGame(CreateGameDTO game);
    Game? GetGameById(int id);
    Game? UpdateGame(int id, UpdateGameDTO game);
    Game? UpdatePartialGame(int id, UpdateGameDTO game);
    void DeleteGame(int id);
    List<Game> BrowseGames(int userId);
}