using TestGameServer.Game.Config;
using TestGameServer.Game.Services.SessionHolder;
using TestGameServer.MessageDispatcher;

namespace TestGameServer.Game.StateMachine.States;

public class WaitForClientConnectionsState : State
{
    private readonly INetworkMessageDispatcher _networkMessageDispatcher;
    private readonly IGameConfiguration _gameConfiguration;
    private readonly SessionProvider _sessionProvider;
    private TaskCompletionSource _completionSource;

    public WaitForClientConnectionsState(
        INetworkMessageDispatcher networkMessageDispatcher,
        IGameConfiguration gameConfiguration,
        SessionProvider sessionProvider
    )
    {
        _networkMessageDispatcher = networkMessageDispatcher;
        _gameConfiguration = gameConfiguration;
        _sessionProvider = sessionProvider;
    }

    public override async Task OnEnter()
    {
        _completionSource = new TaskCompletionSource();
        _networkMessageDispatcher.Subscribe(ENetworkMessage.PlayerConnected, OnPlayerConnected);

        await _completionSource.Task;
        
        StateMachine.ChangeState<WaitForClientLoadedState>();
    }

    private void OnPlayerConnected()
    {
        if (_sessionProvider.Session.Players.Count == _gameConfiguration.RequiredPlayers)
            _completionSource.SetResult();
    }
}