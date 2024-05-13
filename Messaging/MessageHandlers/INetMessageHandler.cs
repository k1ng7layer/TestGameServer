namespace TestGameServer.Messaging.MessageHandlers;

public interface INetMessageHandler
{
    void Subscribe<T>(Action<T> callback) where T : struct;
    void UnSubscribe<T>(Action<T> callback) where T : struct;
}