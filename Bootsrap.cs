using TestGameServer.Game;
using TestGameServer.Game.Config.Game;
using TestGameServer.Network;

namespace TestGameServer;

public class Bootstrap : IDisposable
{
    private readonly IGameConfiguration _configuration;
    private readonly NetworkServer _networkServer;
    private readonly GameCore _gameCore;
    private int _tickRate;
    private DateTime _lastTime;
    private bool _running;

    public Bootstrap(
        IGameConfiguration configuration, 
        GameCore gameCore, 
        NetworkServer networkServer
    )
    {
        _configuration = configuration;
        _gameCore = gameCore;
        _networkServer = networkServer;
    }
    
    public void Initialize()
    {
        _networkServer.Start();
        _gameCore.Initialize();
        _tickRate = 1000 / _configuration.TickRatePerSec;
        _running = true;
    }

    public void Stop()
    {
        _running = false;
    }
    
    public async Task Run()
    {
        while (_running)
        {
            _lastTime = DateTime.UtcNow;
            
            await Task.Delay(_tickRate);
            
            var deltaTime = (float)(DateTime.UtcNow - _lastTime).TotalSeconds;
            //Console.WriteLine($"tick: {deltaTime}");
            
            _networkServer.Tick();
            _gameCore.Tick(deltaTime);
        }
    }

    public void Dispose()
    {
        _running = false; 
        _networkServer.Dispose();
    }
}