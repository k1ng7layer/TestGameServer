using System.Net;
using TestGameServer;
using TestGameServer.Game;
using TestGameServer.Game.Config.Game.Impl;
using TestGameServer.Game.Services.Pathfinding.Impl;
using TestGameServer.MessageDispatcher.Impl;
using TestGameServer.Network;
using TestGameServer.Network.Transport.Impl;

// var path = Path.GetFullPath(@"/Users/paata/Documents/GitRepositories/SampleNetClientDemo/Assets/NavmeshExportNavMesh.obj");
// var geometry = LevelImporter.LoadGeometryFromFile(path);
// var halfEdges = GeometryUtils.TransformFromTriangleToHalfEdge(geometry);
//
// var origin = new Vector2(2.33f, 0.2f);
// //var target = new Vector2(-2.74f, 0.23f);
// var target = new Vector2(0.77f, 2.59f);
//
// foreach (var halfEdge in halfEdges)
// {
//     var p1 = new Vector2(halfEdge.V.Position.X, halfEdge.V.Position.Z);
//     var p2 = new Vector2(halfEdge.nextEdge.V.Position.X, halfEdge.nextEdge.V.Position.Z);
//     
//     if (GeometryUtils.AreLinesIntersecting(origin, target,
//             p1, p2, false))
//     {
//         if (halfEdge.nextEdge.oppositeEdge == null)
//         {
//             Console.WriteLine("cant walk");
//         }
//          
//     }
// }

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

var transport = new LiteNetLibTransport(new IPEndPoint(IPAddress.Any, 5555));
var dispatcher = new NetworkMessageDispatcher();
var pathFindingService = new AStarPathfindingService();

var networkServer = new NetworkServer(transport);
var gameCore = new GameCore(dispatcher, pathFindingService);

var config = new GameConfiguration
{
    TickRatePerSec = 10
};

var bootstrap = new Bootstrap(config, gameCore, networkServer);

bootstrap.Initialize();

await bootstrap.Run();

Console.WriteLine($"server shut down");