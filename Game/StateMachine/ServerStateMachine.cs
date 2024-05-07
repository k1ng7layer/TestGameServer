namespace TestGameServer.Game.StateMachine;

public class ServerStateMachine
{
    private readonly Dictionary<Type, IState> _states = new();
    private IState _currentState;
    
    public void ChangeState<T>() where T : IState
    {
        var newState = _states[typeof(T)];

        try
        {
            ProcessState(newState);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    private async Task ProcessState(IState state)
    {
        if (_currentState != null) 
        {
            await _currentState.Exit();
        }

        _currentState = state;

        await _currentState.Enter();
    }
}