using Scellecs.Morpeh;
using TestGameServer.Game.Config.Game;
using TestGameServer.Game.Ecs.Systems;
using TestGameServer.Messaging.MessageHandlers;
using TestGameServer.Network;

namespace TestGameServer.Game.Ecs.Core;

public class EcsSystemsInstaller
{
    private readonly IGameConfiguration _gameConfiguration;
    private readonly INetMessageHandler _netMessageHandler;
    private readonly NetworkServer _server;
    private readonly List<SystemsGroup> SystemsGroups = new();

    public EcsSystemsInstaller(
        IGameConfiguration gameConfiguration, 
        INetMessageHandler netMessageHandler,
        NetworkServer server
    )
    {
        _gameConfiguration = gameConfiguration;
        _netMessageHandler = netMessageHandler;
        _server = server;
    }
    
    public List<SystemsGroup> CreateSystemGroup(World world)
    {
        var initSystems = world.CreateSystemsGroup();
        var gameGroup = world.CreateSystemsGroup();
        
        initSystems.AddInitializer(new GameStartSystem(_gameConfiguration));
        gameGroup.AddInitializer(new PlayerInputSystem(_netMessageHandler));
        gameGroup.AddSystem(new SpawnMinionsWaveSystem(_server, _gameConfiguration));
        gameGroup.AddSystem(new MinionsSpawnTimerSystem());
 
        SystemsGroups.Add(initSystems);
        SystemsGroups.Add(gameGroup);

        return SystemsGroups;
    }
}