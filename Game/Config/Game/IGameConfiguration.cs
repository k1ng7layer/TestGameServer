using System.Numerics;

namespace TestGameServer.Game.Config.Game;

public interface IGameConfiguration
{
    Vector3 RedTeamMinionsSpawnPoint { get; }
    Vector3 BlueTeamMinionsSpawnPoint { get; }
    int MinionsInWave { get;}
    float MinionsWaveSpawnTimeSec { get;}
    int MaxPlayers { get; }
    int TickRatePerSec { get; }
}