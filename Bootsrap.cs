using System.Net;
using TestGameServer.Game;
using TestGameServer.MessageDispatcher.Impl;
using TestGameServer.Network;
using TestGameServer.Network.Transport.Impl;

namespace TestGameServer;

public class Bootstrap : IDisposable
{
    private NetworkServer _networkServer;
    private GameCore _gameCore;
    
    public void Initialize()
    {
        var transport = new LiteNetLibTransport(new IPEndPoint(IPAddress.Any, 5555));
        var dispatcher = new NetworkMessageDispatcher();
        
        _networkServer = new NetworkServer(transport);
        _gameCore = new GameCore(dispatcher);
    }
    
    private void Tick()
    {
        _networkServer.Tick();
        _gameCore.Tick();
    }

    public void Dispose()
    {
        _networkServer.Dispose();
    }
    
}