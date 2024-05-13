using Scellecs.Morpeh;
using TestGameServer.Game.Config.Game;
using TestGameServer.Game.Ecs.Components;
using TestGameServer.Game.Ecs.Core;
using TestGameServer.Game.Utils;

namespace TestGameServer.Game.Ecs.Systems;

public class GameStartSystem : InitializeSystem
{
    private readonly IGameConfiguration _configuration;

    public GameStartSystem(IGameConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public override void OnAwake()
    {
        var spawnTimerEntity = World.CreateEntity();
        spawnTimerEntity.SetComponent(new SpawnTimerComponent{Value = _configuration.MinionsWaveSpawnTimeSec});

        var gameStateEntity = World.CreateEntity();
        gameStateEntity.SetComponent(new GameStateComponent{Value = EGameState.Running});
    }
}