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
        return Array.Empty<Vector3>();
    }
}