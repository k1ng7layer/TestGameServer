namespace TestGameServer;

public class NetClient
{
    public ushort Id { get; }
    public int ConnectionId { get; set; }

    public NetClient(ushort id)
    {
        Id = id;
    }
}