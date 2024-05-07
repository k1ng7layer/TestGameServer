using TestGameServer.MessageDispatcher;

namespace TestGameServer.Game;

public class GameCore
{
    private readonly INetworkMessageDispatcher _networkMessageDispatcher;
    private readonly Dictionary<int, Player> _players = new();
    private Scene _scene = new();
    
    public GameCore(INetworkMessageDispatcher networkMessageDispatcher)
    {
        _networkMessageDispatcher = networkMessageDispatcher;
    }
    
    public void Tick()
    {
        
    }
} 