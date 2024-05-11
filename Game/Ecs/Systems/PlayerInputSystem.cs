using Scellecs.Morpeh;
using TestGameServer.MessageDispatcher;

namespace TestGameServer.Game.Ecs.Systems;

public class PlayerInputSystem : IInitializer
{
    private readonly INetworkMessageDispatcher _networkMessageDispatcher;

    public PlayerInputSystem(INetworkMessageDispatcher networkMessageDispatcher)
    {
        _networkMessageDispatcher = networkMessageDispatcher;
    }
    
    public void Dispose()
    {
        
    }

    public void OnAwake()
    {
        
    }

    public World World { get; set; }
}