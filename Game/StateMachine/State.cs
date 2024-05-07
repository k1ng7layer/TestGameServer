namespace TestGameServer.Game.StateMachine;

public abstract class State : IState
{
    protected ServerStateMachine StateMachine { get; }
    public Task Enter() => OnEnter();
    public Task Exit() => OnExit();
    public virtual Task OnEnter() => Task.CompletedTask;
    public virtual Task OnExit() => Task.CompletedTask;
}