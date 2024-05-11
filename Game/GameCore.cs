using TestGameServer.Game.Ecs.Core;
using TestGameServer.Game.Services.Pathfinding;
using TestGameServer.Game.StateMachine;
using TestGameServer.MessageDispatcher;

namespace TestGameServer.Game;

public class GameCore
{
    private readonly INetworkMessageDispatcher _networkMessageDispatcher;
    private readonly IPathfindingService _pathfindingService;
    private readonly Dictionary<int, Player> _players = new();
    private readonly ServerStateMachine _serverStateMachine = new();
    private Scene _scene = new();
    private EcsInstaller _ecsInstaller;
    
    public GameCore(
        INetworkMessageDispatcher networkMessageDispatcher, 
        IPathfindingService pathfindingService
    )
    {
        _networkMessageDispatcher = networkMessageDispatcher;
        _pathfindingService = pathfindingService;
    }

    public void Initialize()
    {
        _pathfindingService.Initialize();
        _ecsInstaller = new EcsInstaller();
        _ecsInstaller.Initialize();
    }
    
    public void Tick(float timeStep)
    {
        _ecsInstaller.Tick(timeStep);
    }
} 