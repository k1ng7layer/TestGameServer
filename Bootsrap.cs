using System.Net;
using TestGameServer.Game;
using TestGameServer.Network;
using TestGameServer.Network.Transport.Impl;

namespace TestGameServer;

public class Bootstrap
{
    public void Initialize()
    {
        var transport = new LiteNetLibTransport(new IPEndPoint(IPAddress.Any, 5555));
        var server = new NetworkServer(transport);
        var game = new GameCore();
        
    }
}