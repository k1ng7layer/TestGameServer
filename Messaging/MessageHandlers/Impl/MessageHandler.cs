using TestGameServer.Messaging.MessageHandlers.Serialization;
using TestGameServer.Network.Helpers;

namespace TestGameServer.Messaging.MessageHandlers.Impl;

public class MessageHandler : INetMessageHandler
{
    private readonly IMessageSerializer _messageSerializer;
    private readonly Dictionary<string, List<NetworkMessageHandler>> _registeredHandlersTable = new();

    public MessageHandler(IMessageSerializer messageSerializer)
    {
        _messageSerializer = messageSerializer;
    }
    
    public bool TryGetHandlerId<T>(out string id)
    {
        id = typeof(T).FullName.ToString();

        var hasId = _registeredHandlersTable.TryGetValue(id, out var handlerId);

        return hasId;
    }

    public void Subscribe<T>(Action<T> handler) where T : struct
    {
        var id = typeof(T).FullName;

        if (!_registeredHandlersTable.ContainsKey(id))
        {
            _registeredHandlersTable.Add(id, new List<NetworkMessageHandler>());
        }
            
        var networkHandler = CreateHandler(handler);
        _registeredHandlersTable[id].Add(networkHandler);
    }

    public void UnSubscribe<T>(Action<T> callback) where T : struct
    {
        
    }

    public void CallHandler(string id, ArraySegment<byte> payload)
    {
        //Debug.Log($"CallHandler {id}");
        var hasHandler = _registeredHandlersTable.TryGetValue(id, out var handlers);
            
        if(!hasHandler)
            return;
            
        foreach (var handler in handlers)
        {
            handler?.Invoke(_messageSerializer, payload);
        }
    }
    
    public void CallHandler(ArraySegment<byte> data)
    {
        var byteReader = new SegmentByteReader(data);
        var id = byteReader.ReadString(out _);
        var payloadLength = byteReader.ReadInt32();
        var payload = byteReader.ReadBytes(payloadLength);
       
        var hasHandler = _registeredHandlersTable.TryGetValue(id, out var handlers);
            
        if(!hasHandler)
            return;
            
        foreach (var handler in handlers)
        {
            handler?.Invoke(_messageSerializer, payload);
        }
    }

    private NetworkMessageHandler CreateHandler<T>(Action<T> handler)
        => (deserializer, payload) =>
        {
            var data = deserializer.Deserialize<T>(payload);

            handler?.Invoke(data);
        };
}