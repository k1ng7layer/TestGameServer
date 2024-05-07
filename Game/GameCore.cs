using TestGameServer.Game.StateMachine;
using TestGameServer.Game.StateMachine.States;
using TestGameServer.MessageDispatcher;

namespace TestGameServer.Game;

public class GameCore
{
    private readonly INetworkMessageDispatcher _networkMessageDispatcher;
    private readonly Dictionary<int, Player> _players = new();
    private readonly ServerStateMachine _serverStateMachine = new();
    private Scene _scene = new();
    
    public GameCore(INetworkMessageDispatcher networkMessageDispatcher)
    {
        _networkMessageDispatcher = networkMessageDispatcher;
    }

    public void Initialize()
    {
        _serverStateMachine.ChangeState<WaitForClientConnectionsState>();
    }
    
    public void Tick()
    {
        
    }
} 