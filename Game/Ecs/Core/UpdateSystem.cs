using Scellecs.Morpeh;

namespace TestGameServer.Game.Ecs.Core;

public abstract class UpdateSystem : System, ISystem
{
    public abstract void OnUpdate(float deltaTime);
}