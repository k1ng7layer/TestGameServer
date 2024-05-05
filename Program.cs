using System.Net;
using LiteNetLib;
using LiteNetLib.Utils;
using TestGameServer;

// EventBasedNetListener listener = new EventBasedNetListener();
// NetManager server = new NetManager(listener);
// server.Start(5555 /* port */);
//
// listener.ConnectionRequestEvent += request =>
// {
//     if(server.ConnectedPeersCount < 10 /* max connections */)
//         request.AcceptIfKey("SomeConnectionKey");
//     else
//         request.Reject();
// };
//
// listener.PeerConnectedEvent += peer =>
// {
//     Console.WriteLine("We got connection: {0}", peer);  // Show peer ip
//     NetDataWriter writer = new NetDataWriter();         // Create writer class
//     writer.Put("Hello client!");                        // Put some string
//     peer.Send(writer, DeliveryMethod.ReliableOrdered);  // Send with reliability
// };
//
//
// while (!Console.KeyAvailable)
// {
//     server.PollEvents();
//     Thread.Sleep(15);
// }
// server.Stop();

var transport = new LiteNetLibTransport(new IPEndPoint(IPAddress.Any, 5555));

Server server = new Server(transport);

server.Start();

while (!Console.KeyAvailable)
{
    server.Tick();
    
    Thread.Sleep(15);
}