using TestGameServer.Game.Config.Server;
using TestGameServer.Game.Ecs.Core;

namespace TestGameServer.Game.Ecs.Systems;

public class GameStartSystem : InitializeSystem
{
    private readonly IConfiguration _configuration;

    public GameStartSystem(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public override void OnAwake()
    {
        var minionsSpawnPointA = int.Parse(_configuration.Get("MinionsSpawnPointA"));
        var minionsSpawnPointB = int.Parse(_configuration.Get("MinionsSpawnPointB"));
    }
}