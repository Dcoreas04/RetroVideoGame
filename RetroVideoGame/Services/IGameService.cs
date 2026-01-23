public interface IGameService
{
    Game createGame(CreateGameDTO game);
    Game? getGameById(int id);
    Game? updateGame(int id, UpdateGameDTO game);
    Game? updatePartialGame(int id, UpdateGameDTO game);
    void deleteGame(int id);
}