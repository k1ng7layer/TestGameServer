namespace TestGameServer.MessageDispatcher.Impl;

public class NetworkMessageDispatcher : INetworkMessageDispatcher
{
    private readonly Dictionary<ENetworkMessage, HashSet<Action>> _messageSubscribers = new();
    
    public void Subscribe(ENetworkMessage messageType, Action callback)
    {
        if (!_messageSubscribers.ContainsKey(messageType))
            _messageSubscribers.Add(messageType, new HashSet<Action>());
        
        _messageSubscribers[messageType].Add(callback);
    }

    public void Unsubscribe(ENetworkMessage messageType, Action callback)
    {
        if (!_messageSubscribers.ContainsKey(messageType))
            return;

        _messageSubscribers[messageType].Remove(callback);
    }
}