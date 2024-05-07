namespace TestGameServer.MessageDispatcher;

public interface INetworkMessageDispatcher
{
    void Subscribe(ENetworkMessage messageType, Action callback);
}