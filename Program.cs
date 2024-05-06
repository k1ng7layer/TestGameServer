using System.Net;
using TestGameServer;
using TestGameServer.Game.Helpers;

var path = Path.GetFullPath(@"C:\Users\patat\Documents\NavmeshExport\NavmeshExportNavMesh.obj");
var geometry = LevelImporter.LoadGeometry(path);



// var transport = new LiteNetLibTransport(new IPEndPoint(IPAddress.Any, 5555));
// var server = new Server(transport);
//
// server.Start();
//
// while (!Console.KeyAvailable)
// {
//     server.Tick();
//     
//     Thread.Sleep(15);
// }
//
// server.Dispose();