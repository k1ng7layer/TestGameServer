using System.Numerics;
using OBB;

namespace TestGameServer.Game;

public class GameObject
{
    public Vector3 Position;
    public Quaternion Rotation;

    public Cube Bounds { get; } = new(Vector3.One);
}