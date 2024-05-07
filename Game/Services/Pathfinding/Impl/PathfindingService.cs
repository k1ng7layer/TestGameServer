using System.Numerics;
using TestGameServer.Game.Helpers;
using TestGameServer.Game.Services.Helpers;

namespace TestGameServer.Game.Services.Pathfinding.Impl;

public class NavmeshPathfindingService : IPathfindingService
{
    private Navmesh _navmesh;
    
    public void Initialize()
    {
        var path = Path.GetFullPath(@"/Users/paata/Documents/GitRepositories/SampleNetClientDemo/Assets/NavmeshExportNavMesh.obj");
        var geometry = LevelImporter.LoadGeometryFromFile(path);
        
        _navmesh = new Navmesh
        {
            Triangles = geometry,
            HalfEdges = GeometryUtils.TransformFromTriangleToHalfEdge(geometry)
        };
    }

    public Vector3[] FindPath(Vector3 from, Vector3 to)
    {
        if (!HasDirectWay(from, to))
            return Array.Empty<Vector3>();
        
        return Array.Empty<Vector3>();
    }

    private bool HasDirectWay(Vector3 from, Vector3 to)
    {
        foreach (var halfEdge in _navmesh.HalfEdges)
        {
            var p1 = new Vector2(halfEdge.V.Position.X, halfEdge.V.Position.Z);
            var p2 = new Vector2(halfEdge.nextEdge.V.Position.X, halfEdge.nextEdge.V.Position.Z);
    
            if (GeometryUtils.AreLinesIntersecting(new Vector2(from.X, from.Z), new Vector2(to.X, to.Z),
                    p1, p2, false))
            {
                if (halfEdge?.nextEdge?.oppositeEdge == null)
                {
                    Console.WriteLine("cant walk");
                    return false;
                }
            }
        }

        return true;
    }
}