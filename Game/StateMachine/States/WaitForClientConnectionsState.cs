using TestGameServer.Game.Config.Game;
using TestGameServer.Game.Services.SessionHolder;
using TestGameServer.Network;

namespace TestGameServer.Game.StateMachine.States;

public class WaitForClientConnectionsState : State
{
    private readonly IGameConfiguration _configuration;
    private readonly SessionProvider _sessionProvider;
    private readonly NetworkServer _server;
    private TaskCompletionSource _completionSource;

    public WaitForClientConnectionsState(
        IGameConfiguration configuration,
        SessionProvider sessionProvider,
        NetworkServer server
    )
    {
        _configuration = configuration;
        _sessionProvider = sessionProvider;
        _server = server;
    }

    public override async Task OnEnter()
    {
        _server.ClientConnected += OnPlayerConnected;
        _completionSource = new TaskCompletionSource();
        await _completionSource.Task;
        
        StateMachine.ChangeState<WaitForClientLoadedState>();
    }

    public override Task OnExit()
    {
        _server.ClientConnected -= OnPlayerConnected;
        return Task.CompletedTask;
    }

    private void OnPlayerConnected(NetClient _)
    {
        if (_sessionProvider.Session.Players.Count == _configuration.MaxPlayers)
            _completionSource.SetResult();
    }
}