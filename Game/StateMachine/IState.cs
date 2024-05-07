namespace TestGameServer.Game.StateMachine;

public interface IState
{
    Task Enter();
    Task Exit();
}