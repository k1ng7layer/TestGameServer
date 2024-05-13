using System.Numerics;
using Scellecs.Morpeh;
using TestGameServer.Game.Config.Game;
using TestGameServer.Game.Ecs.Components;
using TestGameServer.Game.Ecs.Core;
using TestGameServer.Game.Utils;
using TestGameServer.Messaging.Helpers;
using TestGameServer.Network;

namespace TestGameServer.Game.Ecs.Systems;

public class SpawnMinionsWaveSystem : UpdateSystem
{
    private readonly NetworkServer _server;
    private readonly IGameConfiguration _gameConfiguration;
    private Filter _filter;
    private ushort _nextId;

    public SpawnMinionsWaveSystem(NetworkServer server, IGameConfiguration gameConfiguration)
    {
        _server = server;
        _gameConfiguration = gameConfiguration;
    }
    
    public override void OnAwake()
    {
        _filter = World.Filter.With<SpawnMinionsComponent>().Build();
    }

    public override void OnUpdate(float deltaTime)
    {
        foreach (var entity in _filter)
        {
            ref var team = ref entity.GetComponent<SpawnMinionsComponent>().Team;
            var minion = World.CreateEntity();
            var position = GetSpawnPosition(team);
            var id = _nextId++;
            minion.AddComponent<MinionComponent>();
            minion.SetComponent(new PositionComponent{Value = position});
            minion.SetComponent(new IdComponent{Value = id});
            _server.SendRawMessage(MessageHelper.SpawnMinionMessage(team, position, id), ESendMode.Reliable);
            
            entity.RemoveComponent<SpawnMinionsComponent>();
        }
    }

    private Vector3 GetSpawnPosition(ETeam team)
    {
        return team == ETeam.Blue
            ? _gameConfiguration.BlueTeamMinionsSpawnPoint
            : _gameConfiguration.RedTeamMinionsSpawnPoint;
    }
}