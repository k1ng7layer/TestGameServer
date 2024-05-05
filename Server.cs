namespace TestGameServer;

public class Server : IDisposable
{
    private readonly Queue<OutcomePendingMessage> _outcomePendingMessages = new();
    private readonly Queue<IncomePendingMessage> _incomePendingMessages = new();
    private readonly Transport _transport;
    private readonly Dictionary<int, NetClient> _netClients = new();
    
    private bool _running;

    public Server(Transport transport)
    {
        _transport = transport;
    }

    public IReadOnlyDictionary<int, NetClient> NetClients => _netClients;

    public void Start()
    {
        _transport.Start();
        _running = true;
        _transport.PeerConnected += OnPeerConnected;
        _transport.DataReceived += OnRawData;
    }
    
    public void Dispose()
    {
        _transport.Stop();
        _transport.PeerConnected -= OnPeerConnected;
        _transport.DataReceived -= OnRawData;
    }

    public void Stop()
    {
        _transport.Stop();
    }

    public void Tick()
    {
        if (!_running)
            return;
        
        _transport.Tick();
        
        SendQueue();
        ReceiveQueue();
    }

    private void OnPeerConnected(int connectionId)
    {
        var id = IdGenerator.Next();
        var client = new NetClient(id);
        client.ConnectionId = connectionId;
        
        _netClients.Add(connectionId, client);
    }
    
    private void OnRawData(int connId, ArraySegment<byte> data)
    {
        var msg = new IncomePendingMessage(data, connId);
            
        _incomePendingMessages.Enqueue(msg);
    }
    
    private void SendQueue()
    {
        while (_outcomePendingMessages.Count > 0)
        {
            var msg = _outcomePendingMessages.Dequeue();
                
            _transport.Send(msg.ConnectionId, msg.Payload, msg.SendMode);
        }
    }
    
    private void ReceiveQueue()
    {
        while (_incomePendingMessages.Count > 0)
        {
            var msg = _incomePendingMessages.Dequeue();

            HandleIncomeMsg(msg.ConnId, msg.Payload);
        }
    }
    
    private void HandleIncomeMsg(int connectionId, ArraySegment<byte> data)
    {
        var messageType = MessageHelper.ReadMessageType(data);

        switch (messageType)
        {
            case ENetworkMessageType.ConnectionRequest:
                HandleConnectionRequest(connectionId, data);
                break;
            case ENetworkMessageType.ClientDisconnected:
                break;
            case ENetworkMessageType.ClientConnected:
                break;
            case ENetworkMessageType.ClientReconnected:
                break;
            case ENetworkMessageType.ClientReady:
                break;
            case ENetworkMessageType.AuthenticationResult:
                break;
            case ENetworkMessageType.NetworkMessage:
                break;
            case ENetworkMessageType.ClientAliveCheck:
                break;
            case ENetworkMessageType.ServerAliveCheck:
                break;
            case ENetworkMessageType.Ping:
                break;
            case ENetworkMessageType.Sync:
                break;
            case ENetworkMessageType.None:
                break;
        }
    }

    private void HandleConnectionRequest(int connId, ArraySegment<byte> data)
    {
        //var connectResult = ConnectionApproveCallback(id, data.Slice(2, data.Count - 2));
        var connectResult = EConnectionResult.Success;
        var byteWriter = new ByteWriter();
        byteWriter.AddUshort((ushort)ENetworkMessageType.AuthenticationResult);
        byteWriter.AddUshort((ushort)connectResult);
        byteWriter.AddInt32(connId);
        byteWriter.AddString("Success");

        SendMessage(connId, byteWriter.Data, ESendMode.Reliable);
    }
    
    private void SendMessage(int connectionId, byte[] data, ESendMode sendMode)
    {
        var message = new OutcomePendingMessage(data, connectionId, sendMode);
            
        _outcomePendingMessages.Enqueue(message);
    }
}