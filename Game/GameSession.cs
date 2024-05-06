namespace TestGameServer.Game;

public class GameSession
{
    private readonly Dictionary<int, Player> _players = new();
    private Scene _scene;
} 