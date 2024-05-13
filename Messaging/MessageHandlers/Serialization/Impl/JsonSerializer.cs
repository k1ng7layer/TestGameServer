namespace TestGameServer.Messaging.MessageHandlers.Serialization.Impl;

public class JsonSerializer : IMessageSerializer
{
    public T Deserialize<T>(ArraySegment<byte> data)
    {
        return (T)System.Text.Json.JsonSerializer.Deserialize<T>(data);
    }
}