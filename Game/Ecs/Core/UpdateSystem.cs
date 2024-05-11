using Scellecs.Morpeh;

namespace TestGameServer.Game.Ecs.Core;

public abstract class UpdateSystem : ISystem
{
    public World World { get; set; }
    
    public virtual void Dispose()
    {}

    public virtual void OnAwake()
    {}
    
    public abstract void OnUpdate(float deltaTime);
}