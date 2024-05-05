using System.Net;
using System.Net.Sockets;
using LiteNetLib;
using LiteNetLib.Utils;

namespace TestGameServer;

public class ExampleServer : INetEventListener
{
    private readonly NetDataWriter _dataWriter;
    private readonly IPEndPoint _localEp;
    private NetManager _server;
    
    public void Start(IPEndPoint endPoint)
    {
        _server = new NetManager(this);
        _server.Start(endPoint.Port);
    }

    public void Stop()
    {
        _server.Stop();
    }

    public void Tick()
    {
        _server.PollEvents();
    }

    public void Send(IPEndPoint endPoint, ArraySegment<byte> data, ESendMode sendMode)
    {
        
    }

    public void Send(int connectionHash, ArraySegment<byte> data, ESendMode sendMode)
    {
        
    }

    public void Dispose()
    {
        _server.Stop();
    }

    public void OnPeerConnected(NetPeer peer)
    {
        Console.WriteLine($"OnPeerConnected, address: {peer.Address}, port: {peer.Port}");
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        Console.WriteLine($"OnPeerDisconnected, address: {peer.Address}, port: {peer.Port}");
 
    }

    public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {
        Console.WriteLine($"OnNetworkError, socketError: {socketError}");
    }

    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
    {
        Console.WriteLine($"OnNetworkReceive, address: {peer.Address}, port: {peer.Port}, data length: {reader.RawData.Length}");
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
        Console.WriteLine($"OnNetworkReceiveUnconnected, address: {remoteEndPoint}");
    }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
        Console.WriteLine($"OnNetworkLatencyUpdate, Peer: {peer.Address}, port: {peer.Port}");
    }

    public void OnConnectionRequest(ConnectionRequest request)
    {
        Console.WriteLine($"OnConnectionRequest, address: {request.RemoteEndPoint}");
        
        request.Accept();
    }
}