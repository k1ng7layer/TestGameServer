using System.Numerics;
using Scellecs.Morpeh;

namespace TestGameServer.Game.Ecs.Components;

public struct PositionComponent : IComponent
{
    public Vector3 Value;
}