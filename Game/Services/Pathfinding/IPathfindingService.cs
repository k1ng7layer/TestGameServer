using System.Numerics;

namespace TestGameServer.Game.Services.Pathfinding;

public interface IPathfindingService
{
    void Initialize();
    Vector3[] FindPath(Vector3 from, Vector3 to);
}