namespace TestGameServer.Messaging.MessageHandlers.Serialization;

public interface IMessageSerializer
{
    T Deserialize<T>(ArraySegment<byte> data);
}