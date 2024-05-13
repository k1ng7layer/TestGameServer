using TestGameServer.Game.Ecs.Core;
using TestGameServer.Messaging.MessageHandlers;

namespace TestGameServer.Game.Ecs.Systems;

public class PlayerInputSystem : InitializeSystem
{
    private readonly INetMessageHandler _netMessageHandler;

    public PlayerInputSystem(INetMessageHandler netMessageHandler)
    {
        _netMessageHandler = netMessageHandler;
    }

    public override void OnAwake()
    {
        Console.WriteLine("OnAwake");
    }
}