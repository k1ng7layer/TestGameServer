using System.Net;
using TestGameServer;

var transport = new LiteNetLibTransport(new IPEndPoint(IPAddress.Any, 5555));
var server = new Server(transport);

server.Start();

while (!Console.KeyAvailable)
{
    server.Tick();
    
    Thread.Sleep(15);
}

server.Dispose();