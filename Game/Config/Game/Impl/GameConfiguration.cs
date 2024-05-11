using System.Numerics;

namespace TestGameServer.Game.Config.Game.Impl;

public class GameConfiguration : IGameConfiguration
{
    public Vector3 RedTeamMinionsSpawnPoint { get; set; }
    public Vector3 BlueTeamMinionsSpawnPoint { get; set; }
    public int MaxPlayers { get; set; }
    public int TickRatePerSec { get; set; }
}