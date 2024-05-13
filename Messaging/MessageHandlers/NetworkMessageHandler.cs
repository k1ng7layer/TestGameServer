using TestGameServer.Messaging.MessageHandlers.Serialization;

namespace TestGameServer.Messaging.MessageHandlers;

public delegate void NetworkMessageHandler(IMessageSerializer serializer, ArraySegment<byte> payload);