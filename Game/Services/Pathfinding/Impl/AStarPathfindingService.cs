using System.Numerics;
using TestGameServer.Game.Helpers;
using TestGameServer.Game.Services.Helpers;

namespace TestGameServer.Game.Services.Pathfinding.Impl;

public class AStarPathfindingService : IPathfindingService
{
    private Navmesh _navmesh;
    private List<TriangleNode> _nodes;
    
    public void Initialize()
    {
        var path = Path.GetFullPath(@"C:\Users\patat\Documents\UnityProjects\SampleNetClient\Assets\NavmeshExportNavMesh.obj");
        var geometry = LevelImporter.LoadGeometryFromFile(path);
        
        _navmesh = new Navmesh
        {
            Triangles = geometry,
            HalfEdges = GeometryUtils.TransformFromTriangleToHalfEdge(geometry)
        };
        
        _nodes = CreateTriangleNodeGraph(_navmesh.Triangles);
    }

    public Vector3[] FindPath(Vector3 from, Vector3 to)
    {
        if (!HasDirectWay(from, to))
            return new[] { from, to };
        
        var startTriangle = FindTriangleByPosition(new Vector2(from.X, from.Z), _nodes);
        var destinationTriangle = FindTriangleByPosition(new Vector2(to.X, to.Z), _nodes);
        
        if (startTriangle == null || destinationTriangle == null)
            return Array.Empty<Vector3>();
        
        startTriangle.CostFromOrigin = 0f;
        
        var reachable = new PriorityQueue<TriangleNode>();
        reachable.Enqueue(startTriangle, 0);
        
        while (reachable.Count > 0)
        {
            var current = reachable.Dequeue();

            if (current == destinationTriangle)
                return TracePath(current, new Vector2(from.X, from.Z), new Vector2(to.X, to.Z));

            foreach (var neighbour in current.Neighbours)
            {
                var newCost = current.CostFromOrigin + EstimateCost(current, neighbour);

                if (newCost < neighbour.CostFromOrigin)
                {
                    neighbour.SharedEdge = FindSharedEdge(neighbour, current);
                        
                    if (neighbour.SharedEdge == null)
                        continue;
                        
                    neighbour.CostFromOrigin = newCost;

                    var priority = newCost + HeuristicFunction(neighbour, new Vector2(to.X, to.Y));
                    neighbour.Previous = current;
                    reachable.Enqueue(neighbour, priority);
                }
            }
        }
        
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
    
    private static Edge FindSharedEdge(TriangleNode a, TriangleNode b)
    {
        int matches = 0;

        Vertex first = null;
        Vertex second = null;
        
        if (GeometryUtils.IsTriangleHasVertex(a.Triangle.Vertex1, b.Triangle))
        {
            first = a.Triangle.Vertex1;
            matches++;
        }
            
        if (GeometryUtils.IsTriangleHasVertex(a.Triangle.Vertex2, b.Triangle))
        {
            if (matches == 0)
                first = a.Triangle.Vertex2;
            else second = a.Triangle.Vertex2;
            
            matches++;
        }
            
        if (matches == 0)
            return null;
            
        if (GeometryUtils.IsTriangleHasVertex(a.Triangle.Vertex3, b.Triangle))
        {
            if (matches == 1)
                second = a.Triangle.Vertex3;
        }

        return new Edge(first, second);
    }
    
    private static float HeuristicFunction(TriangleNode from, Vector2 target)
    {
        return (target - GeometryUtils.CalculateCenterOfTriangle(from.Triangle)).LengthSquared();
    }
    
    private TriangleNode FindTriangleByPosition(Vector2 point, List<TriangleNode> graph)
    {
        foreach (var node in graph)
        {
            var v1 = new Vector2(node.Triangle.Vertex1.Position.X, node.Triangle.Vertex1.Position.Z);
            var v2 = new Vector2(node.Triangle.Vertex2.Position.X, node.Triangle.Vertex2.Position.Z);
            var v3 = new Vector2(node.Triangle.Vertex3.Position.X, node.Triangle.Vertex3.Position.Z);

            if (GeometryUtils.IsPointInTriangle(v1, v2, v3, point))
            {
                return node;
            }
        }

        return null;
    }
    private static List<TriangleNode> CreateTriangleNodeGraph(List<Triangle> triangles)
    {
        //create nodes
        var nodes = new List<TriangleNode>();
        foreach (var triangle in triangles)
        {
            nodes.Add(new TriangleNode(triangle));
        }

        for (int i = 0; i < nodes.Count; i++)
        {
            var triangleNode1 = nodes[i];
            var vertices = new List<Vector3>();
                
            vertices.Add(triangleNode1.Triangle.Vertex1.Position);
            vertices.Add(triangleNode1.Triangle.Vertex2.Position);
            vertices.Add(triangleNode1.Triangle.Vertex3.Position);

            for (int j = 0; j < nodes.Count; j++)
            {
                if (i == j) continue;
                
                var triangleNode2 = nodes[j];
    
                int passed = 0;
          
                foreach (var vertex in vertices)
                {
                    if (triangleNode2.Triangle.Vertex1.Position == vertex)
                        passed++;
                        
                    if (triangleNode2.Triangle.Vertex2.Position == vertex)
                        passed++;
                        
                    if (triangleNode2.Triangle.Vertex3.Position == vertex)
                        passed++;
                }
                    
                if (passed == 2)
                    triangleNode1.Neighbours.Add(triangleNode2);
            }
        }

        return nodes;
    }
    
    private static Vector3[] TracePath(
        TriangleNode node, 
        Vector2 from, 
        Vector2 to)
    {
        var nodePath = new List<TriangleNode>();
           
        int pathLength = 2;
            
        while (node.Previous != null)
        {
            nodePath.Add(node);
            node = node.Previous;
            pathLength++;
        }
            
        var path = new Vector3[pathLength];
        int remain = pathLength;
        path[pathLength - 1] = new Vector3(to.X, 0f, to.Y);
        path[0] = new Vector3(from.X, 0f, from.Y);
        remain -= 1;

        for (var i = 0; i < nodePath.Count; i++)
        {
            var triangleNode = nodePath[i];
            
            var edgeOrigin = triangleNode.SharedEdge.V2;
            var edgeEnd = triangleNode.SharedEdge.V1;
            var fromPoint = path[pathLength - 1 - i];
            var point = GeometryUtils.FindNearestPointOnLine(
                new Vector2(edgeOrigin.Position.X, edgeOrigin.Position.Z),
                new Vector2(edgeEnd.Position.X, edgeEnd.Position.Z), new Vector2(fromPoint.X, fromPoint.Z));

            path[--remain] = new Vector3(point.X, 0f, point.Y);
        }

        return path;
    }
    
    private float EstimateCost(TriangleNode from, TriangleNode to)
    {
        return to.LocationCost;
    }
    
}