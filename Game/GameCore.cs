using TestGameServer.Game.Config.Game;
using TestGameServer.Game.Ecs.Core;
using TestGameServer.Game.Models;
using TestGameServer.Game.Services.Pathfinding;
using TestGameServer.Game.StateMachine;
using TestGameServer.Messaging.MessageHandlers;
using TestGameServer.Network;

namespace TestGameServer.Game;

public class GameCore : IDisposable
{
    private readonly INetMessageHandler _netMessageHandler;
    private readonly IPathfindingService _pathfindingService;
    private readonly IGameConfiguration _gameConfiguration;
    private readonly NetworkServer _server;
    private readonly Dictionary<int, Player> _players = new();
    private readonly ServerStateMachine _serverStateMachine = new();
    private Scene _scene = new();
    private EcsBehaviour _ecsBehaviour;
    
    public GameCore(
        INetMessageHandler netMessageHandler, 
        IPathfindingService pathfindingService,
        IGameConfiguration gameConfiguration,
        NetworkServer server
    )
    {
        _netMessageHandler = netMessageHandler;
        _pathfindingService = pathfindingService;
        _gameConfiguration = gameConfiguration;
        _server = server;
    }

    public void Initialize()
    {
        _pathfindingService.Initialize();
        _ecsBehaviour = new EcsBehaviour(new EcsSystemsInstaller(_gameConfiguration, _netMessageHandler, _server));
        _ecsBehaviour.Initialize();
        _server.ClientConnected += ServerOnClientConnected;
        //_serverStateMachine.ChangeState<WaitForClientConnectionsState>();
    }

    private void ServerOnClientConnected(NetClient obj)
    {
        if (_server.NetClients.Count == _gameConfiguration.MaxPlayers)
            _ecsBehaviour.Enable();
    }

    public void Tick(float timeStep)
    {
        _ecsBehaviour.Tick(timeStep);
    }

    public void Dispose()
    {
        _server.ClientConnected -= ServerOnClientConnected;
        _ecsBehaviour.Dispose();
    }
} 