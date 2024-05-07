﻿using System.Net;

namespace TestGameServer.Network.Transport;

public abstract class Transport : IDisposable
{
    public abstract event Action<int, ArraySegment<byte>> DataReceived;
    public abstract event Action<int> PeerConnected;
    public abstract void Start();
    public abstract void Stop();
    public abstract void Tick();
    public abstract void Send(IPEndPoint endPoint, ArraySegment<byte> data, ESendMode sendMode);
    public abstract void Send(int connectionHash, ArraySegment<byte> data, ESendMode sendMode);
    public abstract void Dispose();
}